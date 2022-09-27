using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDisconnectedLobbyRoom : IResponseEvent
{
    public LobbyPlayer LobbyPlayer { get; private set; }
    public OnDisconnectedLobbyRoom(LobbyPlayer lobbyPlayer)
    {
        this.LobbyPlayer = lobbyPlayer;
    }

    public void Invoke(EventManagerBase eventManagerBase)
    {

        var lobbyManager = eventManagerBase as LobbyManager;
        MainUIManager.Instance.GetPanel<LobbyPanel>().RemoveRoom(LobbyPlayer.UserName);
    }
}
