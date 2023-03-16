using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
[System.Serializable]
public struct Team
{
    public NetworkedGameManager.TeamTypes team;
    public List<GamePlayObject> gamePlayObjects;

    public Team(NetworkedGameManager.TeamTypes team, List<GamePlayObject> gamePlayObjects)
    {
        this.team = team;
        this.gamePlayObjects=gamePlayObjects;

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
public class GamePlayObject
{
    public int connectionId;
    public NetworkIdentity netIdentity;

}
