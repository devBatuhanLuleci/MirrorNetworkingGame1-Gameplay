using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static NetworkedGameManager;

public class GemModeNetworkedGameManager : NetworkedGameManager
{

    private NetworkSpawnObjectInInterval NetworkSpawnObjectInInterval;

    private Dictionary<TeamTypes, List<GemData>> collectedCrystalDictionary;


    //TODO: disable custom attribute 'unu yaz.  [Disable]  Unity Learn bak.
    [TextArea]
    public string teams;

    public override void Awake()
    {
        base.Awake();

        if (TryGetComponent<NetworkSpawnObjectInInterval>(out NetworkSpawnObjectInInterval networkSpawnObjectInInterval))
        {

            NetworkSpawnObjectInInterval = networkSpawnObjectInInterval;

        }

    }
    public override void SetupClient()
    {
        base.SetupClient();
        Invoke("SetMyTeam", 3);

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

        var teamType = GetMyTeam(connectionID);

        OnGemModeCrystalValueChanged(teamType, collectedCrystalDictionary[teamType].Count);
        #region old 


        //var currentTeam = Teams.Find(t => t.team.Equals(teamType));
        //var otherTeam = Teams.Where(t => !t.team.Equals(currentTeam)).First();

        //foreach (var team in Teams)
        //{
        //    foreach (var player in team.teamPlayers)
        //    {
        //        OnGemModeCrystalValueChanged(player.netIdentity.connectionToClient, team.Equals(teamType), collectedCrystalDictionary[teamType].Count);
        //    }
        //}

        //  OnGemModeCrystalValueChanged(player.netIdentity.connectionToClient, true, gems.Count);
        //foreach (var item in Teams)
        //{

        //    if (collectedCrystalDictionary.TryGetValue(item.team, out var gems))
        //    {
        //        OnGemModeCrystalValueChanged(teamType.Equals(item.team), gems.Count);
        //    }
        //    //if ()
        //    //{

        //    //}
        //}
        //foreach (var player in currentTeam.teamPlayers)
        //{
        //    Debug.Log($" current team: {currentTeam.team} player name:  {player.netIdentity.transform.name}   conneID:{player.connectionId} ");

        //    if(collectedCrystalDictionary.TryGetValue(currentTeam.team,out var gems)){

        //        OnGemModeCrystalValueChanged(player.netIdentity.connectionToClient, true, gems.Count);

        //    }
        //}



        //foreach (var player in currentTeam.teamPlayers)
        //{
        //    Debug.Log($" current team: {currentTeam.team} player name:  {player.netIdentity.transform.name}   conneID:{player.connectionId} ");

        //    if(collectedCrystalDictionary.TryGetValue(currentTeam.team,out var gems)){

        //        OnGemModeCrystalValueChanged(player.netIdentity.connectionToClient, true, gems.Count);

        //    }
        //}

        //foreach (var player in otherTeam.teamPlayers)
        //{
        //    Debug.Log($" current team: {otherTeam.team} player name:  {player.netIdentity.transform.name}   conneID:{player.connectionId} ");

        //    if (collectedCrystalDictionary.TryGetValue(otherTeam.team, out var gems))
        //    {

        //        OnGemModeCrystalValueChanged(player.netIdentity.connectionToClient, false, gems.Count);

        //    }
        //}

        #endregion



    }
    void SetMyTeam()
    {

        myTeam = GetMyTeam(NetworkClient.localPlayer);
        CmdGetTeamInfo();
    }

    [Command(requiresAuthority = false)]
    public void CmdGetTeamInfo()
    {

        foreach (var item in collectedCrystalDictionary)
        {
            OnGemModeCrystalValueChanged(item.Key, collectedCrystalDictionary[item.Key].Count);

        

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
    public void OnGemModeCrystalValueChanged(TeamTypes team, int gemCount)
    {


        if (GetMyTeam() == team)
        {
            GameplayPanelUIManager.Instance.GemModeGameplayCanvas.GetComponentInChildren<TeamUIPanelManager>().AllyTeamPanel.ChangeCrystalAmountText(gemCount);

        }
        else
        {
            GameplayPanelUIManager.Instance.GemModeGameplayCanvas.GetComponentInChildren<TeamUIPanelManager>().EnemyTeamPanel.ChangeCrystalAmountText(gemCount);

        }

    }



}


