using kcp2k;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static NetworkedGameManager;

public class NetworkedGameManager : NetworkBehaviour
{
    #region Sub classes

    #endregion
    #region 

    #region Singleton 
    private static NetworkedGameManager instance;
    public static NetworkedGameManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    private bool IsClient => ACGDataManager.Instance.GameData.TerminalType == TerminalType.Client;


    [SyncVar(hook = nameof(OnGameStarted))]
    [HideInInspector]
    public bool isGameStarted;

    [SyncVar]
    public bool isGameFinished;

    [HideInInspector] public bool isClientConnected = false;


    //public bool IsGameStarted
    //{
    //    get { Debug.Log("değer" + isGameStarted); return isGameStarted; }
    //    set { IsGameStartable(value); }
    //}

    public TeamTypes myTeam;

    public enum TeamTypes { Team1, Team2 }
    [SyncVar]
    public List<Team> Teams = new List<Team>();

    #region MonoBehaviour Methods
    public virtual void Awake()
    {
        instance = this;
        InitAssigments();
        Info("awake: " + MatchNetworkManager.Instance.mode);
    }
    private void Start()
    {
       
        Info("isClient: " + isClient);
        if (IsClient)
        {
            SetupClient();
            isClientConnected = true;
        }
        if (isServer)
        {
            MatchNetworkManager.Instance.OnPlayerListChanged.AddListener(OnCharacterReplaced);

        }
    }
    public virtual void Update()
    {
        if (!isServer) { return; }
    }
    public virtual void OnGameStarted(bool oldValue, bool newValue)
    {
        Debug.Log("Boolean value changed from " + oldValue + " to " + newValue);
    }


    public virtual void OnDestroy()
    {
        if (isServer)
        {
            MatchNetworkManager.Instance.OnPlayerListChanged.RemoveListener(OnCharacterReplaced);

        }
    }
    private void OnCharacterReplaced(Dictionary<int, NetworkConnectionToClient> players)
    {
        CreateTeam(players);
    }


    #endregion

    #region NetworkBehavior Methods

    #endregion
    #endregion

    private void InitAssigments()
    {

    }
   

    public virtual void SetupClient()
    {

        GameplayPanelUIManager.Instance.AutoSelectCharacter();
        //   GameplayPanelUIManager.Instance.SelectCharacter();
        ClientStarted();

        CmdReady();
        Info("awake: " + MatchNetworkManager.Instance.mode);

    }

    public TeamTypes GetMyTeam(int connID)
    {
        TeamTypes teamType;

        var teamEnum = from personGroup in Teams
                       from person in personGroup.teamPlayers
                       where person.connectionId.Equals(connID)
                       select personGroup;

        teamType = teamEnum.First().team;

        //  Debug.Log($" my team: {teamType}");

        return teamType;
    }

    public TeamTypes GetMyTeam()
    {

        return myTeam;
    }

    public TeamTypes GetMyTeam(NetworkIdentity networkIdentity)
    {
        TeamTypes teamType;

        var teamEnum = from personGroup in Teams
                       from person in personGroup.teamPlayers
                       where person.netIdentity.Equals(networkIdentity)
                       select personGroup;

        teamType = teamEnum.First().team;

        //  Debug.Log($" my team: {teamType}");

        return teamType;
    }


    public void CreateTeam(Dictionary<int, NetworkConnectionToClient> players)
    {


        bool isTeam1 = true;
        Teams = new List<Team>() {
            new Team(TeamTypes.Team1,new List<TeamPlayers>())
            {

            },
            new Team(TeamTypes.Team2,new List<TeamPlayers>())
            {

            }


        };

        foreach (var item in players)
        {
            var team = isTeam1 ? TeamTypes.Team1 : TeamTypes.Team2;

            var myTeam = Teams.Where(t => t.team == team).FirstOrDefault();

            TeamPlayers teamPlayers = new TeamPlayers(item.Value.connectionId, item.Value.identity
            //{
            //    //new ItemTypes
            //    //    {
            //    //    connectionId=item.Value.connectionId,
            //    //    netIdentity=item.Value.identity,
            //    //    lalam=ItemTypes.ItemType.Characters
            //    //    }
            //}
            );

            myTeam.teamPlayers.Add(teamPlayers);


            isTeam1 = !isTeam1;
        }





    }



    public bool IsInMyTeam(NetworkIdentity otherPlayer)
    {

        var result3 = from personGroup in Teams
                      from person in personGroup.teamPlayers
                      where person.netIdentity.Equals(NetworkClient.localPlayer)
                      select personGroup;

        var ourTeam = result3.First();

        var result4 = from personGroup in Teams
                      from person in personGroup.teamPlayers
                      where person.netIdentity.Equals(otherPlayer)
                      select personGroup;
        var otherTeam = result4.First();

        return ourTeam.team == otherTeam.team;

    }
    public bool IsInMyTeam(uint otherPlayerNetId)
    {
        var result3 = from personGroup in Teams
                      from person in personGroup.teamPlayers
                      where person.netIdentity.netId.Equals(NetworkClient.localPlayer.netId)
                      select personGroup;

        var ourTeam = result3.First();


        var result4 = from personGroup in Teams
                      from person in personGroup.teamPlayers
                      where person.netIdentity.netId.Equals(otherPlayerNetId)
                      select personGroup;

        var otherTeam = result4.First();


        return ourTeam.team == otherTeam.team;


    }

    public bool IsInMyTeam(uint ownerNetID, uint otherPlayerNetID)
    {


        //var result = from personGroup in Teams
        //             from person in personGroup.teamPlayers
        //             where person.itemType.Where(t => t.Equals(t.netIdentity.netId)).Equals(ownerNetID)
        //             select personGroup;
        //var ourTeam = result.First();


        var result3 = from personGroup in Teams
                      from person in personGroup.teamPlayers
                      where person.netIdentity.netId.Equals(ownerNetID)
                      select personGroup;

        var ourTeam = result3.First();

        var result2 = from personGroup in Teams
                      from person in personGroup.teamPlayers
                      where person.netIdentity.netId.Equals(otherPlayerNetID)
                      select personGroup;
        var otherTeam = result2.First();



        return ourTeam.team == otherTeam.team;


    }

    [Command/*(requiresAuthority =true)*/]
    public void GetLocalPlayer(/*NetworkConnectionToClient conn*/)
    {

        int connectionId = NetworkClient.connection.connectionId;
        //  Debug.Log("id :" + connectionId);
        Debug.Log("id :" + NetworkConnection.LocalConnectionId);
        //foreach (var player in FindObjectsOfType<PlayerController>())
        //{
        //    player.c
        //}
        // Debug.Log(conn.);

        //foreach (var item in players.Values)
        //{
        //    if (item.identity.isLocalPlayer)
        //    {


        //    //if (item.connectionId== conn.connectionId)
        //    //{
        //        //  Debug.Log($"isim: { item.identity.name}  id:     {item.identity.netId}");
        //        return item.identity.GetComponent<PlayerController>();
        //    //}
        //    }
        //}
        //if(conn.identity.TryGetComponent<PlayerController>(out PlayerController playerController))
        //{
        //    return playerController;
        //}


    }

    public void ClientStarted()
    {

        string msg = $"Client Started. Port: {ACGDataManager.Instance.GameData.Port}";
        Info(msg);
    }
    public virtual void ServerStarted(Dictionary<int, NetworkConnectionToClient> players)
    {
        CreateTeam(players);
        // Setup();
        string msg = $" <color=green> Server listining on </color> localhost:{ACGDataManager.Instance.GameData.Port}";
        Info(msg);
    }


    public void StartGame()
    {
        Info("StartGame");
        OnGameStarted();
    }
    private void OnGameStarted()
    {
       
        GameplayPanelUIManager.Instance.DeactivateUltiButton();


    }
    public void Info(string msg)
    {
        msg = "[MatchNetworkManager]: " + msg;
        GameplayPanelUIManager.Instance.GetPanel<Waiting>().Info = msg;
        Debug.LogError(msg);

    }

    #region RPCMethods
    [ClientRpc]
    public virtual void RpcStartGame()
    {
        Info("RpcStartGame");

        StartGame();
    }
    #endregion
    #region Command Methods

    public int playerCount = 0;

    [Command(requiresAuthority = false)]
    public void CmdReady()
    {
        Info("Ready!");
        playerCount++;


        SetPlayerCount(playerCount);

        if (playerCount >= ACGDataManager.Instance.GameData.MaxPlayerCount)
        {

            RpcStartGame();
        }
    }

    // TODO: mak playerCount a SyncVar
    [ClientRpc]
    public void SetPlayerCount(int value)
    {
        this.playerCount = value;
    }




    #endregion

    #region Team Creation

    #endregion

}


