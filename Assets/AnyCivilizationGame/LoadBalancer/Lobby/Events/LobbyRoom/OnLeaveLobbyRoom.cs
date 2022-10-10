using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLeaveLobbyRoom : IResponseEvent
{
    public LobbyPlayer LobbyPlayer { get; private set; }
    public OnLeaveLobbyRoom(LobbyPlayer lobbyPlayer)
    {
        this.LobbyPlayer = lobbyPlayer;
    }

    public void Invoke(EventManagerBase eventManagerBase)
    {

        var lobbyManager = eventManagerBase as LobbyManager;
        MainUIManager.Instance.GetPanel<LobbyPanel>().LeaveRoom(LobbyPlayer.UserName);
        if (lobbyManager.LobbyPlayer.UserName == LobbyPlayer.UserName)
        {
            lobbyManager.LobbyPlayer = null;
        }
    }
}
