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

    Dictionary<int, int> playerGems;

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
        Invoke("StartSpawnLoop", 3);


        collectedCrystalDictionary = new Dictionary<TeamTypes, List<GemData>>();
        playerGems = new Dictionary<int, int>();

        //playerGems = new Dictionary<int, int>();

    }
    public void StartSpawnLoop()
    {
        NetworkSpawnObjectInInterval.StartSpawnLoop();


    }
    public void OnGemCollected(int connectionID)
    {
        AddToCollectedCrystalList(connectionID);
        var teamType = GetMyTeam(connectionID);

        OnGemModeCrystalValueChanged(teamType, collectedCrystalDictionary[teamType].Count);

        PlayerController player = MatchNetworkManager.Instance.GetPlayerByConnectionID(connectionID);
        player.OnCrystalCollected_UpdatePlayer(AddGemDataToPlayers().GetValueOrDefault(connectionID));
        ShowPlayerGemsLog();

    }
    public void OnGemDroppedByThisPlayer(int connectionID)
    {
  
        PlayerController player = MatchNetworkManager.Instance.GetPlayerByConnectionID(connectionID);
        var teamType = GetMyTeam(connectionID);

        var TotalCrystalToDrop = playerGems[connectionID];
        Debug.Log($"totalDroppableGems of this player :  {TotalCrystalToDrop}");
        for (int i = 0; i < TotalCrystalToDrop; i++)
        {
            NetworkSpawnObjectInInterval.SpawnObjectWithDiffrentForce(player.transform.position);

        }

        RemoveFromCollectCrystalList(connectionID, teamType);

        OnGemModeCrystalValueChanged(teamType, collectedCrystalDictionary[teamType].Count);


        var RemainedAmountCrystalOfPlayer = AddGemDataToPlayers().GetValueOrDefault(connectionID);



        player.OnCrystalRemoved_UpdatePlayer(RemainedAmountCrystalOfPlayer);


        ShowPlayerGemsLog();

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
    public void RemoveFromCollectCrystalList(int connectionID, TeamTypes MyTeamType)
    {


        foreach (var item in collectedCrystalDictionary[MyTeamType].ToArray())
        {
            if (item.connID.Equals(connectionID))
            {

                collectedCrystalDictionary[MyTeamType].Remove(item);
            }
        }
  
    }



    public Dictionary<int, int> AddGemDataToPlayers()
    {
        playerGems = new Dictionary<int, int>();


        foreach (var team in collectedCrystalDictionary)
        {
            foreach (var data in team.Value)
            {

                if (playerGems.ContainsKey(data.connID))
                {
                    playerGems[data.connID]++;
                }
                else
                {
                    playerGems.Add(data.connID, 1);
                }

            }


        }

        return playerGems;

    }

    public void ShowPlayerGemsLog()
    {

        teams = "";
        foreach (var player in playerGems)
        {
            var message = $"{GetMyTeam(player.Key)} : playerID: {player.Key} gemAmount : {player.Value} ";
            teams += message + "\n";

        }

    }

    public void RemoveGemFromPlayer()
    {
        //if (collectedCrystalDictionary.TryGetValue(gemData.teamTypes, out var teamList))
        //{
        //    collectedCrystalDictionary.(gemData.teamTypes, new List<GemData>() { gemData });

        //}

    }

    [ClientRpc]
    public void OnGemModeCrystalValueChanged(TeamTypes team, int gemCount)
    {


        if (GetMyTeam() == team)
        {
            GameplayPanelUIManager.Instance.GemModeGameplayCanvas.GetComponentInChildren<TeamUIPanelManager>().AllyTeamPanel.ChangeCrystalAmountUI(gemCount);

        }
        else
        {
            GameplayPanelUIManager.Instance.GemModeGameplayCanvas.GetComponentInChildren<TeamUIPanelManager>().EnemyTeamPanel.ChangeCrystalAmountUI(gemCount);

        }

    }



}


