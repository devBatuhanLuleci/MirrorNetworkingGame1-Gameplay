using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayer 
{
    public bool IsLeader { get; private set; } = false;
    public bool IsReady { get; set; } = false;
    public string UserName { get; private set; }

    public LobbyPlayer(string userName)
    {
        UserName = userName;
    } 
    public LobbyPlayer(string userName, bool isLeader, bool isReady) : this(userName)
    {
        IsLeader = isLeader;
        IsReady = isReady;
    }
}
