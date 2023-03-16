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


    public enum TeamTypes { Team1, Team2 }
    [SyncVar]
    public List<Team> Teams = new List<Team>();

    #region MonoBehaviour Methods
    private void Awake()
    {
        instance = this;
        InitAssigments();
        Info("awake: " + MatchNetworkManager.Instance.mode);
    }
    private void Start()
    {

        Info("isClient: " + isClient);
        if (IsClient) SetupClient();
        if (isServer)
        {
            MatchNetworkManager.Instance.OnPlayerListChanged.AddListener(OnCharacterReplaced);

        }
    }



    public void OnDestroy()
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


    private void SetupClient()
    {
        // TODO: open character select panel
        //    GameUIManager.Instance.DeactivateJoystickButtons();
        GameplayPanelUIManager.Instance.SelectCharacter();
        ClientStarted();
        CmdReady();
        Info("awake: " + MatchNetworkManager.Instance.mode);

    }

    public void CreateTeam(Dictionary<int, NetworkConnectionToClient> players)
    {


        bool isTeam1 = true;
        Teams = new List<Team>() {
            new Team(TeamTypes.Team1,new List<GamePlayObject>())
            {
                //team=TeamTypes.Team1,
                //gamePlayObjects=new List<GamePlayObject>()
            },
            new Team(TeamTypes.Team2,new List<GamePlayObject>())
            {

             //team=TeamTypes.Team2,
             //   gamePlayObjects=new List<GamePlayObject>()

            }


        };

        foreach (var item in players)
        {
            var team = isTeam1 ? TeamTypes.Team1 : TeamTypes.Team2;
           
                var myTeam = Teams.Where(t => t.team == team).FirstOrDefault();
                myTeam.gamePlayObjects.Add(new GamePlayObject()
                {
                    connectionId = item.Value.connectionId,
                    netIdentity = item.Value.identity
                });
            
            
            //for (int i = 0; i < Teams.Count; i++)
            //{
            //    team = new Team()
            //    {
            //        team = isTeam1 ? TeamTypes.Team1 : TeamTypes.Team2,
            //        connectionId = item.Value.connectionId,
            //        netIdentity = item.Value.identity
            //    });
            isTeam1 = !isTeam1;
        }





    }



    public bool IsInMyTeam(NetworkIdentity otherPlayer)
    {
  
        var result = from personGroup in Teams
                     from person in personGroup.gamePlayObjects
                     where person.netIdentity.Equals(NetworkClient.localPlayer)
                     select personGroup;
        var ourTeam = result.First();



        var result2 = from personGroup in Teams
                      from person in personGroup.gamePlayObjects
                      where person.netIdentity.Equals(otherPlayer)
                      select personGroup;
        var otherTeam = result2.First();

        return ourTeam.team == otherTeam.team;
    
    }
    public bool IsInMyTeam(uint otherPlayerNetId)
    {
        var result = from personGroup in Teams
                     from person in personGroup.gamePlayObjects
                     where person.netIdentity.netId.Equals(NetworkClient.localPlayer.netId)
                     select personGroup;
       var ourTeam= result.First();



        var result2 = from personGroup in Teams
                     from person in personGroup.gamePlayObjects
                     where person.netIdentity.netId.Equals(otherPlayerNetId)
                     select personGroup;
        var otherTeam = result2.First();


        return ourTeam.team == otherTeam.team;
 

    }

    public bool IsInMyTeam(uint ownerNetID, uint otherPlayerNetID)
    {


        var result = from personGroup in Teams
                     from person in personGroup.gamePlayObjects
                     where person.netIdentity.netId.Equals(ownerNetID)
                     select personGroup;
        var ourTeam = result.First();



        var result2 = from personGroup in Teams
                      from person in personGroup.gamePlayObjects
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
    public void ServerStarted(Dictionary<int, NetworkConnectionToClient> players)
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
    public void RpcStartGame()
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


