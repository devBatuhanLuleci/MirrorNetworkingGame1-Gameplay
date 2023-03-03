using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoinedToLobbyRoom : IResponseEvent
{
    public int RoomCode { get; private set; }
    public LobbyPlayer LobbyPlayer;
    public LobbyPlayer[] LobbyPlayers;

    public PlayerJoinedToLobbyRoom(int roomCode, LobbyPlayer[] lobbyPlayers, LobbyPlayer lobbyPlayer)
    {
        RoomCode = roomCode;
        LobbyPlayers = lobbyPlayers;
        LobbyPlayer = lobbyPlayer;
    }
    public void Invoke(EventManagerBase eventManagerBase)
    {
        var lobbyManager = eventManagerBase as LobbyManager;

        Debug.Log("PlayerJoinedToLobbyRoom " + LobbyPlayer.UserName);

        lobbyManager.LobbyPlayer = LobbyPlayer;
        MainPanelUIManager.Instance.GetPanel<LobbyPanel>().JoinedRoom(RoomCode, LobbyPlayer.UserName);
        for (int i = 0; i < LobbyPlayers.Length; i++)
        {
            var player = LobbyPlayers[i];
            if (player.UserName != lobbyManager.LobbyPlayer.UserName)
                MainPanelUIManager.Instance.GetPanel<LobbyPanel>().JoinRoom(player.UserName);
        }
    }
}
