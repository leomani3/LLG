public struct PlayerData
{
    public string PlayerName { get; private set; }
    public ulong ClientId { get; private set; }
    public int Color { get; private set; }

    public PlayerData(string playerName, ulong clientId)
    {
        PlayerName = playerName;
        ClientId = clientId;
        Color = -1;
    }

    public void SetColor(int c)
    {
        Color = c;
    }
}
