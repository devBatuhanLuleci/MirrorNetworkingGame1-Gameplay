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
        NetworkSpawnObjectInInterval?.StartSpawnLoop();


        collectedCrystalDictionary = new Dictionary<TeamTypes, List<GemData>>();
        playerGems = new Dictionary<int, int>();

        //playerGems = new Dictionary<int, int>();

    }

    public void OnGemCollected(int connectionID)
    {
        AddToCollectedCrystalList(connectionID);
        ShowTeamGemValuesInInspector();
       
        var teamType = GetMyTeam(connectionID);


        OnGemModeCrystalValueChanged(teamType, collectedCrystalDictionary[teamType].Count);

        PlayerController player = MatchNetworkManager.Instance.GetPlayerByConnectionID(connectionID);
        player.OnCrystalCollectedUpdatePlayer(AddGemDataToPlayers().GetValueOrDefault(connectionID));

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
        foreach (var item in playerGems)
        {
            PlayerController player = MatchNetworkManager.Instance.GetPlayerByConnectionID(item.Key);
            player.OnCrystalCollectedUpdatePlayer(AddGemDataToPlayers().GetValueOrDefault(item.Key));

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



        //if (collectedCrystalDictionary.TryGetValue(gemData.teamTypes, out var gemDatas))
        //{
        //    // teamList2.Add(gemData);
        //    foreach (var gemdata in gemDatas)
        //    {
        //        playerGems.TryAdd(gemdata.connID, playerGems.GetValueOrDefault(gemdata.connID) + 1);
        //    }

        //}
        //foreach (var player in playerGems)
        //{
        //    Debug.Log($"playerID: {player.Key}  gemAmount: {player.Value} ");
        //}
    }

    public void ShowTeamGemValuesInInspector()
    {
        //teams = "";
        //foreach (var team in collectedCrystalDictionary)
        //{
        //    var message = $"{team.Key} : {team.Value.Count} ";
        //    teams += message + "\n";

        //}

    }
    public Dictionary<int, int> AddGemDataToPlayers()
    {
        playerGems = new Dictionary<int, int>();


        teams = "";

        foreach (var team in collectedCrystalDictionary)
        {
            foreach (var data in team.Value)
            {
               // playerGems.TryAdd(data.connID, 0);

                if(playerGems.ContainsKey(data.connID))
                {
                    playerGems[data.connID]++;
                }
                else
                {
                    playerGems.Add(data.connID, 1);
                }

            }


        }
        foreach (var player in playerGems)
        {
            var message = $"{GetMyTeam(player.Key)} : playerID: {player.Key} gemAmount : {player.Value} ";
            teams += message + "\n";

        }
        return playerGems;

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


