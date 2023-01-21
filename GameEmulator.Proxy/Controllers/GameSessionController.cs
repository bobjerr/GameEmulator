using GameEmulator.Grains.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameEmulator.Proxy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameSessionController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;

        public GameSessionController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [HttpPost]
        public Task Apply(GameEvent gameEvent)
        {
            var grain = _clusterClient.GetGrain<IGameSessionGrain>(gameEvent.PlayerId);
            return grain.Apply(gameEvent);
        }
    }
}
