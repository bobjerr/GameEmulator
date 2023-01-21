namespace GameEmulator.Grains.Interfaces
{
    public interface IGameSessionGrain : IGrainWithIntegerKey
    {
        public Task Apply(GameEvent gameEvent);
    }
}