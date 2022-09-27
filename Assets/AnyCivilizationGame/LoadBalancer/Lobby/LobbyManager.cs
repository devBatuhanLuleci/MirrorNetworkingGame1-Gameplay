using ACGAuthentication;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : EventManagerBase
{
    public override LoadBalancerEvent loadBalancerEvent { get; protected set; } = LoadBalancerEvent.Lobby;
    public LobbyPlayer LobbyPlayer { get; set; }

    public LobbyManager(LoadBalancer loadBalancer) : base(loadBalancer)
    {
        loadBalancer.AddEventHandler(loadBalancerEvent, this);

    }
    ~LobbyManager()
    {
        loadBalancer.RemoveEventHandler(loadBalancerEvent, this);
    }
    internal override Dictionary<byte, Type> initResponseTypes()
    {
        var responseTypes = new Dictionary<byte, Type>();
        responsesByType.Add((byte)LobbyEvent.StartMatnch, typeof(StartMatchEvent));
        responsesByType.Add((byte)LobbyEvent.GetPlayers, typeof(GetPlayersEvent));
        responsesByType.Add((byte)LobbyEvent.CreateLobbyRoom, typeof(CreateLobbyRoom));
        responsesByType.Add((byte)LobbyEvent.JoinLobbyRoom, typeof(JoinLobbyRoom));
        responsesByType.Add((byte)LobbyEvent.NewJoinedToLobbyRoom, typeof(NewPlayerJoinedToLobbyRoom));
        responsesByType.Add((byte)LobbyEvent.JoinedToLobbyRoom, typeof(PlayerJoinedToLobbyRoom));
        responsesByType.Add((byte)LobbyEvent.MaxPlayerError, typeof(MaxPlayerError));
        responsesByType.Add((byte)LobbyEvent.OnDisconnectedLobbyRoom, typeof(OnDisconnectedLobbyRoom));

        return responsesByType;
    }

}