using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
[System.Serializable]
public struct Team
{
    public NetworkedGameManager.TeamTypes team;
    public int  connectionId;
    public NetworkIdentity netIdentity;


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
