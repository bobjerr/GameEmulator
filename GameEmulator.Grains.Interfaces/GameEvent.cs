using System.Text.Json.Serialization;

namespace GameEmulator.Grains.Interfaces
{
    [GenerateSerializer]
    [JsonDerivedType(typeof(GameEvent), typeDiscriminator: "baseEvent")]
    [JsonDerivedType(typeof(CreateAObject), typeDiscriminator: "A")]
    [JsonDerivedType(typeof(CreateBObject), typeDiscriminator: "B")]
    [JsonDerivedType(typeof(RemoveObject), typeDiscriminator: "Remove")]
    public record GameEvent(long PlayerId);

    [GenerateSerializer]
    public record CreateAObject(long PlayerId, int Value) : GameEvent(PlayerId);

    [GenerateSerializer]
    public record CreateBObject(long PlayerId, string Name) : GameEvent(PlayerId);

    [GenerateSerializer]
    public record RemoveObject(long PlayerId) : GameEvent(PlayerId);
}