using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static NetworkedGameManager;

public class CrystalModeNetworkedGameManager : NetworkedGameManager
{


    private NetworkSpawnObjectInInterval NetworkSpawnObjectInInterval;
    private CrystalModeCountdown crystalModeCountdown;
    private CrystalModeGameTime crystalModeGameTime;
    private CrystalModeGamePanelsHandler crystalModeGamePanelsHandler;
    private Dictionary<TeamTypes, List<GemData>> collectedCrystalDictionary;
    public TeamTypes currentlyWinningTeam;
    public int maxCrystalAmount = 10;
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
        if (TryGetComponent<CrystalModeCountdown>(out CrystalModeCountdown crystalModeCountdown))
        {

            this.crystalModeCountdown = crystalModeCountdown;

        }
        if (TryGetComponent<CrystalModeGameTime>(out CrystalModeGameTime crystalModeGameTime))
        {

            this.crystalModeGameTime = crystalModeGameTime;

        }
        if (TryGetComponent<CrystalModeGamePanelsHandler>(out CrystalModeGamePanelsHandler crystalModeUIOpeningHandler))
        {

            this.crystalModeGamePanelsHandler = crystalModeUIOpeningHandler;

        }

        //if (isServer)
        //{

        crystalModeGamePanelsHandler.onHandleOpeningPanelReadyToSpawnCrystalAction.AddListener(OnSequenceIsReadyForSpawnCrystal);
        crystalModeGamePanelsHandler.onHandleOpeningPanelReadyToPlayAction.AddListener(OnSequenceIsReadyForPlay);
        // }
        //cystalModeGamePanelsHandler.onCountDownPanelActivation.AddListener(OnCurrentTeamReachedMaxGemAmount);


        crystalModeGamePanelsHandler.CreateCrystalModeCanvas();


    }
    public override void Update()
    {
        if (!isServer) { return; }
        base.Update();
        if (Input.GetKeyDown(KeyCode.N) && crystalModeGamePanelsHandler.gamePanelStatus != CrystalModeGamePanelsHandler.GamePanelStatus.CountDown)
        {

            OnCurrentTeamReachedMaxGemAmount();
        }
        if (Input.GetKeyDown(KeyCode.B) && crystalModeGamePanelsHandler.gamePanelStatus == CrystalModeGamePanelsHandler.GamePanelStatus.CountDown)
        {
            CancelCountDown();


        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitilizeTeamOfThePlayer();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            OnGameFinished();
        }

    }
    public void OnSequenceIsReadyForSpawnCrystal()
    {
        StartSpawnLoop();
    }
    public void OnSequenceIsReadyForPlay()
    {
        crystalModeGameTime.StartCountDown();
        isGameStarted = true;

    }
    public override void OnGameStarted(bool oldValue, bool newValue)
    {
        base.OnGameStarted(oldValue, newValue);

        if (newValue == true)
        {

            ActivateJoystickOnClients();


        }
    }

    public void OnGameFinished()
    {
        OnFinishedDoSomeChanges();
        crystalModeGamePanelsHandler.DoWinnerTeamText(currentlyWinningTeam.ToString());
        crystalModeGamePanelsHandler.isCountDownTextPanelActive = false;
        crystalModeGamePanelsHandler.isFinishPanelActive = true;
    }

    public void OnFinishedDoSomeChanges()
    {
        isGameFinished = true;
        isGameStarted = false;

    }

    public void OnGameTimeFinished_FinishGame()
    {
        OnFinishedDoSomeChanges();
    }


    public void ActivateJoystickOnClients()
    {
        //InputHandler.Instance.joystickCanvas
        GameplayPanelUIManager.Instance.joystickCanvas.ShowSmoothly();
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        crystalModeGamePanelsHandler.onHandleOpeningPanelReadyToSpawnCrystalAction.RemoveListener(OnSequenceIsReadyForSpawnCrystal);
        crystalModeGamePanelsHandler.onHandleOpeningPanelReadyToPlayAction.RemoveListener(OnSequenceIsReadyForPlay);

    }
    public override void OnStartClient()
    {
        base.OnStartClient();

    }
    public override void SetupClient()
    {
        base.SetupClient();



    }
    public override void OnStartServer()
    {

        //Oluşturduğumuz gemmodegameplaycanvas varsa onu server'dan sildir.
        if (GameplayPanelUIManager.Instance.GemModeGameplayCanvas != null)
        {
            Destroy(GameplayPanelUIManager.Instance.GemModeGameplayCanvas.gameObject);
        }

        base.OnStartServer();
        
    }

    public override void ServerStarted(Dictionary<int, NetworkConnectionToClient> players)
    {
        base.ServerStarted(players);


        collectedCrystalDictionary = new Dictionary<TeamTypes, List<GemData>>();
        playerGems = new Dictionary<int, int>();
        currentlyWinningTeam = TeamTypes.None;

    }
    [ClientRpc]
    public override void RpcStartGame()
    {
        base.RpcStartGame();

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

        CheckTeamCrystalAmountReachedMax(teamType);
        //if (CheckTeamCrystalAmountToReachMax(MyTeamCrystalScore(teamType)) && crystalModeGamePanelsHandler.gamePanelStatus != CrystalModeGamePanelsHandler.GamePanelStatus.CountDown)
        //{
        //    currentlyWinningTeam = teamType;
        //    OnCurrentTeamReachedMaxGemAmount();


        //}

    }
    public void CheckTeamCrystalAmountReachedMax( TeamTypes teamType)
    {

        if (currentlyWinningTeam!= teamType && CheckTeamCrystalAmountToReachMax(MyTeamCrystalScore(teamType)) && crystalModeGamePanelsHandler.gamePanelStatus != CrystalModeGamePanelsHandler.GamePanelStatus.CountDown)
        {
            currentlyWinningTeam = teamType;
            crystalModeGamePanelsHandler.DoWinnableTeamCountDownText(currentlyWinningTeam.ToString());
            OnCurrentTeamReachedMaxGemAmount();


        }
    }
    public void OnGemDroppedByThisPlayer(int connectionID)
    {


        if (playerGems.ContainsKey(connectionID))
        {

            // devam eden işlemler

            PlayerController player = MatchNetworkManager.Instance.GetPlayerByConnectionID(connectionID);
            var teamType = GetMyTeam(connectionID);

            var TotalCrystalToDrop = playerGems[connectionID];
            //Debug.Log("Total düşürülücek gem miktarı");
            //Debug.Log($"totalDroppableGems of this player :  {TotalCrystalToDrop}");
            for (int i = 0; i < TotalCrystalToDrop; i++)
            {
                NetworkSpawnObjectInInterval.SpawnObjectWithDiffrentForce(player.transform.position);

            }

            if (currentlyWinningTeam.Equals(teamType) && CheckTeamCrystalAmountToReachMax(MyTeamCrystalScore(teamType)) && crystalModeGamePanelsHandler.gamePanelStatus == CrystalModeGamePanelsHandler.GamePanelStatus.CountDown)
            {
               // Debug.Log("cancelladım");
                CancelCountDown();
                currentlyWinningTeam = TeamTypes.None;
                CheckTeamCrystalAmountReachedMax(GetOtherTeam(teamType));


            }
            RemoveFromCollectCrystalList(connectionID, teamType);

            OnGemModeCrystalValueChanged(teamType, collectedCrystalDictionary[teamType].Count);

            var RemainedAmountCrystalOfPlayer = AddGemDataToPlayers().GetValueOrDefault(connectionID);



            player.OnCrystalRemoved_UpdatePlayer(RemainedAmountCrystalOfPlayer);


            ShowPlayerGemsLog();

        }
        else
        {
            Debug.Log("hiç gemi yoktu.");
            // anahtar yoksa yapılacak işlemler
        }




    }
    public TeamTypes GetOtherTeam(TeamTypes myTeam)
    {
        TeamTypes otherTeam = TeamTypes.None;

        if (myTeam.Equals(TeamTypes.Blue))
        {
            otherTeam = TeamTypes.Red;
        }
        else if (myTeam.Equals(TeamTypes.Red))
        {
            otherTeam = TeamTypes.Blue;

        }
        return otherTeam;
    }
    public int MyTeamCrystalScore(TeamTypes teamType)
    {
        if (collectedCrystalDictionary.ContainsKey(teamType))
        {

            int crystalScore = collectedCrystalDictionary[teamType].Count;
            Debug.Log($" {teamType.ToString()} takımının crystalScore'u: {crystalScore} ");
            return crystalScore;
        }
        else return 0;
    }
    public bool CheckTeamCrystalAmountToReachMax(int teamCrystalAmount)
    {
        bool isReached = false;

        isReached = teamCrystalAmount >= maxCrystalAmount;

        return isReached/*= teamCrystalAmount.Equals(maxCrystalAmount)*/;

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

    public void OnCurrentTeamReachedMaxGemAmount()
    {
        //cystalModeGamePanelsHandler
        // crystalModeCountdown.StartCountDown();
        crystalModeGamePanelsHandler.StartToCountDownSequenceForATeam();
    }
    public void CancelCountDown()
    {
        crystalModeGamePanelsHandler.CancelCountDownPanel();

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
        //:TODO  burayı getcompenenintchildren'dan kurtar.

        if (GetMyTeam() == team)
        {
            GameplayPanelUIManager.Instance.GemModeGameplayCanvas.GetComponentInChildren<CrystalStatsUIPanelManager>().AllyTeamPanel.ChangeCrystalAmountUI(gemCount);

        }
        else
        {
            GameplayPanelUIManager.Instance.GemModeGameplayCanvas.GetComponentInChildren<CrystalStatsUIPanelManager>().EnemyTeamPanel.ChangeCrystalAmountUI(gemCount);

        }

    }



}


