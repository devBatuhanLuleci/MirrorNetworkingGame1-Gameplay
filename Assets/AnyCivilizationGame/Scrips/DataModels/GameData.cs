[System.Serializable]
public class GameData
{
    public ushort Port = 0;
    public string GameServerAddress = "localhost";
    public string HostAddress;
    public TerminalType TerminalType = TerminalType.Client;
}

public enum TerminalType
{
    Server,
    Client
}
