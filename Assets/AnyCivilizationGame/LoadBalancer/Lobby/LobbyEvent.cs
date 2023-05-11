using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LobbyEvent : byte {
    StartMatnch = 0x01,
    GetPlayers = 0x2,
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
    GetClanNames = 0xE, //clan panel icin eklendi
    OnGetClanNames = 0xF,
    SendClanName =0x10,
    OnSendClanName =0x11,
    GetFriendNames = 0x12,
    OnGetFriendNames = 0x13,
    SendFriendName = 0x14,
    OnSendFriendName = 0x15,
    SendNotificationInfo = 0x16,
    OnSendNotificationInfo = 0x17,
    SendAccessTokenKey=0x18,
    OnSendAccessTokenKey=0x19

}