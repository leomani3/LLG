using MLAPI.Serialization;

public struct LobbyPlayerState : INetworkSerializable
{
    public ulong ClientId;
    public string PlayerName;
    public bool IsReady;
    public int NumberColor;

    public LobbyPlayerState(ulong clientId, string playerName, bool isReady, int numberColor)
    {
        ClientId = clientId;
        PlayerName = playerName;
        IsReady = isReady;
        NumberColor = numberColor;
    }

    public void NetworkSerialize(NetworkSerializer serializer)
    {
        serializer.Serialize(ref ClientId);
        serializer.Serialize(ref PlayerName);
        serializer.Serialize(ref IsReady);
        serializer.Serialize(ref NumberColor);
    }
}
