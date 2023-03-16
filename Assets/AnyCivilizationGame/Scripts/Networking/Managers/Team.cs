using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
[System.Serializable]
public struct Team
{
    public NetworkedGameManager.TeamTypes team;
    public List<TeamPlayers> teamPlayers;

    public Team(NetworkedGameManager.TeamTypes team, List<TeamPlayers> teamPlayers)
    {
        this.team = team;
        this.teamPlayers=teamPlayers;

    }
    //public Team(string teamName)
    //{
   
    //    if (Enum.TryParse<MatchNetworkManager.TeamTypes>(teamName, true, out team))
    //    {
    //        players=new List<GameObject>();
    //        // ignore cases
    //       Debug.Log($"teamname: {teamName}  , team : {team}" );
    //    }

    //}
}
[System.Serializable]
public struct TeamPlayers
{

   

    public List<ItemTypes> itemType;

    public TeamPlayers(List<ItemTypes> itemType)
    {
        this.itemType = itemType;
    }   
}

[System.Serializable]
public struct ItemTypes
{
  
    public enum ItemType { Characters, Objects };
    public ItemType lalam;
    public int connectionId;
    public NetworkIdentity netIdentity;
}