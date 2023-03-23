using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemModeNetworkedGameManager : NetworkedGameManager
{

    private NetworkSpawnObjectInInterval NetworkSpawnObjectInInterval;
 
    private Dictionary<TeamTypes, List<GemData>> collectedCrystalDictionary;

 
    //TODO: disable custom attribute 'unu yaz.  [Disable]  Unity Learn bak.
    [TextArea]
    public string teams ;

    public override void Awake()
    {
        base.Awake();
          
        if (TryGetComponent<NetworkSpawnObjectInInterval>(out NetworkSpawnObjectInInterval networkSpawnObjectInInterval))
        {

            NetworkSpawnObjectInInterval = networkSpawnObjectInInterval;

        }
      
    }


    public override void ServerStarted(Dictionary<int, NetworkConnectionToClient> players)
    {
        base.ServerStarted(players);
        NetworkSpawnObjectInInterval?.StartSpawnLoop();


        collectedCrystalDictionary = new Dictionary<TeamTypes, List<GemData>>();
     

    }

    public void OnGemCollected(int connectionID)
    {
        AddToCollectedCrystalList(connectionID);
        ShowTeamGemValuesInInspector();

        var teamType =  GetMyTeam(connectionID);


        //OnGemModeCrystalValueChanged()
        var currentTeam=  Teams.Find(t => t.team.Equals(teamType));

        
        foreach (var player in currentTeam.teamPlayers)
        {
            Debug.Log($" current team: {currentTeam.team} player name:  {player.netIdentity.transform.name}   conneID:{player.connectionId} ");
        }

    }
    public void AddToCollectedCrystalList(int connectionID)
    {
        var gemData = new GemData(connectionID, GetMyTeam(connectionID));


        if (collectedCrystalDictionary.TryGetValue(gemData.teamTypes, out var teamList))
        {
            teamList.Add(gemData);

        }
        else
        {
            collectedCrystalDictionary.Add(gemData.teamTypes, new List<GemData>() { gemData });
        }
    }

    public void ShowTeamGemValuesInInspector()
    {
        teams = "";
        foreach (var team in collectedCrystalDictionary)
        {
            var message = $"{team.Key} : {team.Value.Count} ";
            teams += message + "\n";

        }

    }
    [ClientRpc]
    public void OnGemModeCrystalValueChanged(bool isAlly, int gemvalue)
    {
        if (isAlly)
        {
        GameplayPanelUIManager.Instance.GemModeGameplayCanvas.GetComponentInChildren<TeamUIPanelManager>().AllyTeamPanel.ChangeCrystalAmountText(gemvalue);

        }
        else
        {
        GameplayPanelUIManager.Instance.GemModeGameplayCanvas.GetComponentInChildren<TeamUIPanelManager>().EnemyTeamPanel.ChangeCrystalAmountText(gemvalue);

        }

    }

}
