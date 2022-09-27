using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinLobbyRoom : IEvent
{
    public int RoomCode { get; set; }

    public JoinLobbyRoom(int roomCode)
    {
        RoomCode = roomCode;
    }
}
