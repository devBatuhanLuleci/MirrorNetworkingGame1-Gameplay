using ACGAuthentication;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class SpawnServer : EventManagerBase
{
    public Dictionary<ushort, Room> rooms = new Dictionary<ushort, Room>();
    public const ushort START_PORT = 3000;
    private ushort currentPort = START_PORT;
    public override LoadBalancerEvent loadBalancerEvent { get; protected set; } = LoadBalancerEvent.SpawnServer;

    public static ILog log = LogManager.GetLogger(typeof(SpawnServer));

    public SpawnServer(LoadBalancer loadBalancer) : base(loadBalancer)
    {
        loadBalancer.AddEventHandler(loadBalancerEvent, this);
        Debug("SpawnServer initilized!");
    }
    ~SpawnServer()
    {
        loadBalancer.RemoveEventHandler(loadBalancerEvent, this);
    }

    public void Debug(string msg)
    {
        log.Debug(msg);

    }

    internal override Dictionary<byte, Type> initResponseTypes()
    {
        var responseTypes = new Dictionary<byte, Type>();
        responseTypes.Add((byte)SpawnServerEvent.Ready, typeof(OnReadyEvent));
        responseTypes.Add((byte)SpawnServerEvent.ConnectToGameServer, typeof(ConnectToGameServerEvent));
        responseTypes.Add((byte)SpawnServerEvent.CloseRoom, typeof(CloseRoomEvent));
        responseTypes.Add((byte)SpawnServerEvent.RoomInfoEvent, typeof(RoomInfoEvent));

        return responseTypes;
    }



    public void NewMatch(ClientPeer client)
    {
        // TODO:  Start new game server and forward players to server
        var gameServer = StartGameServer(currentPort);

        if (gameServer != null)
        {
            var newRoom = new Room(currentPort, gameServer);
            newRoom.AddPlayer(client);
            rooms.Add(currentPort, newRoom);
            currentPort++;
        }
        else
        {
            throw new Exception("Game Server can't start.");
        }

    }




    public Process StartGameServer(int port)
    {
        return ExecuteManager.ExecuteCommand(port.ToString());
    }
}
