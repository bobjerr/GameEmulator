namespace GameEmulator.Grains
{
    public abstract class GameObject
    {
    }

    public class AObject : GameObject
    {
        public int Value { get; set; }
    }

    public class BObject : GameObject
    {
        public string Name { get; set; }
    }
}