using MLAPI.Serialization;

public struct LevelStateObject : INetworkSerializable
{
    public bool IsTriggered;

    public LevelStateObject(bool isTriggered)
    {
        IsTriggered = isTriggered;
    }

    public void NetworkSerialize(NetworkSerializer serializer)
    {
        serializer.Serialize(ref IsTriggered);
    }
}
