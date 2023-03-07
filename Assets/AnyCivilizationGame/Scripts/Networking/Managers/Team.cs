using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team
{
    public MatchNetworkManager.TeamTypes team;
    public List<Transform> players;



    public Team(string teamName)
    {
   
        if (Enum.TryParse<MatchNetworkManager.TeamTypes>(teamName, true, out team))
        {
        
            // ignore cases
            Console.WriteLine("teamname: {0}  , team : {1}", teamName, team);
        }

    }
}
