using MLAPI.Serialization;

public struct AvailableLevelState : INetworkSerializable
{
    public int LevelNumber;
    public bool IsAvailable;
    public bool IsDone;

    public AvailableLevelState(int levelNumber, bool isAvailable, bool isDone = false)
    {
        LevelNumber = levelNumber;
        IsAvailable = isAvailable;
        IsDone = isDone;
    }

    public void NetworkSerialize(NetworkSerializer serializer)
    {
        serializer.Serialize(ref LevelNumber);
        serializer.Serialize(ref IsAvailable);
        serializer.Serialize(ref IsDone);
    }
}