using GameEmulator.Grains.Interfaces;
using Orleans.Runtime;

namespace GameEmulator.Grains
{
    internal class GameSessionGrain : Grain, IGameSessionGrain
    {
        private readonly IPersistentState<GameState> _gameState;

        public GameSessionGrain([PersistentState("GameState", "GameStateStore")] IPersistentState<GameState> gameState)
        {
            _gameState = gameState;
        }

        public override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            var key = this.GetPrimaryKeyLong();
            Console.WriteLine($"{DateTime.Now}. Session {key} has been activated.");

            if (!_gameState.RecordExists)
            {
                _gameState.State.PlayerId = key;
                await _gameState.WriteStateAsync();
            }

            await base.OnActivateAsync(cancellationToken);
        }

        public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
        {
            var key = this.GetPrimaryKeyLong();
            Console.WriteLine($"{DateTime.Now}. Session {key} has been deactivated.");

            return base.OnDeactivateAsync(reason, cancellationToken);
        }

        public async Task Apply(GameEvent gameEvent)
        {
            ApplyEvent(gameEvent);
            _gameState.State.Version++;
            await _gameState.WriteStateAsync();
        }

        private void ApplyEvent(GameEvent gameEvent)
        {
            switch (gameEvent)
            {
                case CreateAObject createAObject:
                    _gameState.State.Objects.Add(new AObject() { Value = createAObject.Value });
                    break;
                case CreateBObject createBObject:
                    _gameState.State.Objects.Add(new BObject() { Name = createBObject.Name });
                    break;
                case RemoveObject _:
                    if (_gameState.State.Objects.Count > 0)
                    {
                        _gameState.State.Objects.RemoveAt(0);
                    }
                    break;
                default:
                    Console.WriteLine("Unsupported event");
                    break;
            }
        }
    }
}
