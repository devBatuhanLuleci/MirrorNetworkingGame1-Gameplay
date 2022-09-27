using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerJoinedToLobbyRoom : IResponseEvent
{
    public LobbyPlayer Player { get; private set; }

    public NewPlayerJoinedToLobbyRoom(LobbyPlayer player)
    {
        this.Player = player;
    }

    public void Invoke(EventManagerBase eventManagerBase)
    {
        Debug.Log($"{Player.UserName} has been joined");
        MainUIManager.Instance.GetPanel<LobbyPanel>().JoinRoom(Player.UserName);
    }
}