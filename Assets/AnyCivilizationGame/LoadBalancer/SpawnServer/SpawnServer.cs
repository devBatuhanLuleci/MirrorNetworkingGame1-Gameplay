using ACGAuthentication;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class SpawnServer : EventManagerBase
{
    public Dictionary<ushort, Room> rooms = new Dictionary<ushort, Room>();
    public const ushort START_PORT = 3000;
    private ushort currentPort = START_PORT;
    public override LoadBalancerEvent loadBalancerEvent { get; protected set; } = LoadBalancerEvent.SpawnServer;

    public SpawnServer(LoadBalancer loadBalancer) : base(loadBalancer)
    {
        loadBalancer.AddEventHandler(loadBalancerEvent, this);
    }
    ~SpawnServer()
    {
        loadBalancer.RemoveEventHandler(loadBalancerEvent, this);
    }

    internal override Dictionary<byte, Type> initResponseTypes()
    {
        var responseTypes = new Dictionary<byte, Type>();
        responseTypes.Add((byte)SpawnServerEvent.Ready, typeof(OnReadyEvent));
        responseTypes.Add((byte)SpawnServerEvent.ConnectToGameServer, typeof(ConnectToGameServerEvent));
        responseTypes.Add((byte)SpawnServerEvent.CloseRoom, typeof(CloseRoomEvent));

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
