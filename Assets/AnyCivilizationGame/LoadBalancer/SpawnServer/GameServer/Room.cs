
using System.Collections.Generic;
using System.Diagnostics;


[System.Serializable]
public class Room
{
    #region Public fields
    public ushort Port { get; private set; }
    public ClientPeer peer { get; private set; }

    public RoomState State { get; private set; } = RoomState.Preparing;

    #endregion

    #region Private Fields
    private List<ClientPeer> players = new List<ClientPeer>();

    #endregion

    #region Instance
    public Process GameServer => gameServer;
    private Process gameServer;
    #endregion
    public Room(ushort port, Process server)
    {
        Port = port;
        gameServer = server;
    }


    public void AddPlayer(ClientPeer player)
    {
        players.Add(player);
        player.OnDissconnect += RemovePlayer;
    }
    public void RemovePlayer(ClientPeer player)
    {
        player.OnDissconnect -= RemovePlayer;
        players.Remove(player);
    }
    public void CloseRoom()
    {
        // TODO: not direct kill send kill message
        // if can't response from room kill the room instance
        GameServer.Kill();

    }

    internal void ConnectPlayers()
    {
        players.ForEach(el => ConnectPlayer(el));
    }

    internal void ConnectPlayer(ClientPeer player)
    {
        //var roomManager = player.identity.GetComponent<RoomManager>();
        //roomManager.RpcSetReady(port);
        if (State == RoomState.Ready)
        {
            State = RoomState.Started;
        }
    }

}
