public struct PlayerData
{
    public string PlayerName { get; private set; }
    public ulong ClientId { get; private set; }
    public int NumberColor { get; private set; }

    public PlayerData(string playerName, ulong clientId, int numberColor)
    {
        PlayerName = playerName;
        ClientId = clientId;
        NumberColor = numberColor;
    }
}
