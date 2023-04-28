using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
[System.Serializable]
public struct Team
{
    public NetworkedGameManager.TeamTypes teamType;
    public List<TeamPlayers> teamPlayers;

    public Team(NetworkedGameManager.TeamTypes team, List<TeamPlayers> teamPlayers)
    {
        this.teamType = team;
        this.teamPlayers=teamPlayers;

    }
 
}
[System.Serializable]
public struct TeamPlayers
{

   

  
    public int connectionId;
    public NetworkIdentity netIdentity;

    public TeamPlayers(int connID,NetworkIdentity netIdentity)
    {
        connectionId = connID;
        this.netIdentity = netIdentity;
    }   
}

