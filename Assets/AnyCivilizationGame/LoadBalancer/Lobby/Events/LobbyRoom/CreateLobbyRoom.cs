using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyRoomCreated : IResponseEvent
{
    public LobbyPlayer LobbyPlayer;
    public int RoomCode { get; private set; }

    public LobbyRoomCreated(int roomCode, LobbyPlayer lobbyPlayer)
    {
        RoomCode = roomCode;
        LobbyPlayer = lobbyPlayer;

    }

    public void Invoke(EventManagerBase eventManagerBase)
    {
        var lobbyManager = eventManagerBase as LobbyManager;

        Debug.Log($"CreateLobbyRoom id: {RoomCode}");

        lobbyManager.LobbyPlayer = LobbyPlayer;
        MainPanelUIManager.Instance.GetPanel<LobbyPanel>().CreateRoom(RoomCode, LobbyPlayer.UserName);
    }
}
