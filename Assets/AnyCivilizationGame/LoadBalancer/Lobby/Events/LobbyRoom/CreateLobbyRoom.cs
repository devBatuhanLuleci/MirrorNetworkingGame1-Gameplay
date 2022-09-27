using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLobbyRoom : IResponseEvent
{
    public LobbyPlayer LobbyPlayer;
    public int RoomCode { get; private set; }

    public CreateLobbyRoom(int roomCode, LobbyPlayer lobbyPlayer)
    {
        RoomCode = roomCode;
        LobbyPlayer = lobbyPlayer;
    }

    public void Invoke(EventManagerBase eventManagerBase)
    {
        var lobbyManager = eventManagerBase as LobbyManager;

        Debug.Log($"CreateLobbyRoom id: {RoomCode}");

        lobbyManager.LobbyPlayer = LobbyPlayer;
        MainUIManager.Instance.GetPanel<LobbyPanel>().CreateRoom(RoomCode, LobbyPlayer.UserName);
    }
}
