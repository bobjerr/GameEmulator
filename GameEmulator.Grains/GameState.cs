namespace GameEmulator.Grains
{
    [Serializable]
    [GenerateSerializer]
    public class GameState
    {
        public Guid Id { get; set; }
        public long PlayerId { get; set; }
        public int Version { get; set; }
        public List<GameObject> Objects { get; set; } = new List<GameObject>();
    }
}
