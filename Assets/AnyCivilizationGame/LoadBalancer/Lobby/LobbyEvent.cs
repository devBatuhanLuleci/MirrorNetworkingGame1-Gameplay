using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LobbyEvent : byte
{
    StartMatnch = 0x01,
    GetPlayers  = 0x2,
    CreateLobbyRoom = 0x3,
    JoinLobbyRoom = 0x4,
    NewJoinedToLobbyRoom = 0x5,
    JoinedToLobbyRoom = 0x6,
    MaxPlayerError = 0x7,
    OnLeaveLobbyRoom = 0x8,
    ReadyStateChange = 0x9,
    ReadyStateChanged = 0xA,
    LeaveRoom = 0xB,
    ThereIsNoRoom = 0xC,
    StartLobbyRoom = 0xD,
}
