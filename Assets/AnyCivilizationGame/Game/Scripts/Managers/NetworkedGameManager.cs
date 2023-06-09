using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static MatchNetworkManager;

public class NetworkedGameManager : NetworkBehaviour
{
    #region Sub classes

    #endregion

    #region Singleton 
    protected static NetworkedGameManager instance;
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
    [HideInInspector] public bool isGameStarted;

    [SyncVar] public bool isGameFinished;

    [HideInInspector] public bool isClientConnected = false;

    public TeamTypes clientTeam;

    public enum TeamTypes { Blue, Red, None }
    [SyncVar]
    public List<Team> Teams = new List<Team>();

    #region MonoBehaviour Methods
    public virtual void Awake()
    {
        instance = this;
        Info("awake: " + MatchNetworkManager.Instance.mode);
    }
    public virtual void Start()
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
        Debug.Log("OnGameStarted value changed from " + oldValue + " to " + newValue);
    }


    public virtual void OnDestroy()
    {
        if (isServer)
        {
            MatchNetworkManager.Instance.OnPlayerListChanged.RemoveListener(OnCharacterReplaced);
        }
    }
    private void OnCharacterReplaced(Dictionary<int, MatchPeer> players)
    {
        CreateTeam(players);
    }


    #endregion


    public virtual void SetupClient()
    {

        GameplayPanelUIManager.Instance.AutoSelectCharacter();
        //   GameplayPanelUIManager.Instance.SelectCharacter();
        ClientStarted();
        var user = AuthenticationManager.Instance.User;
        CmdReady(user.accessToken);
        Info("awake: " + MatchNetworkManager.Instance.mode);

    }
    public override void OnStartServer()
    {
        base.OnStartServer();

    }
    public override void OnStartClient()
    {
        base.OnStartClient();
    }



    public TeamTypes GetMyTeam(int connID)
    {
        TeamTypes teamType;

        var teamEnum = from personGroup in Teams
                       from person in personGroup.teamPlayers
                       where person.connectionId.Equals(connID)
                       select personGroup;

        teamType = teamEnum.FirstOrDefault().teamType;

        //  Debug.Log($" my team: {teamType}");

        return teamType;
    }
    public void SetThisClientTeam(TeamTypes team)
    {
        clientTeam = team;

    }
    public TeamTypes GetMyTeam()
    {
        return clientTeam;
    }

    public TeamTypes GetMyTeam(NetworkIdentity networkIdentity)
    {
        TeamTypes teamType;
        var teamEnum = from personGroup in Teams
                       from person in personGroup.teamPlayers
                       where person.netIdentity.Equals(networkIdentity)
                       select personGroup;
        teamType = teamEnum.FirstOrDefault().teamType;
        return teamType;
    }

    public void CreateTeam(Dictionary<int, MatchPeer> players)
    {
        //bool isTeam1 = true;
        Teams = new List<Team>() { };

        var teams = LoadBalancer.Instance.LobbyManager.roomData;
        var TeamBlue = new Team(TeamTypes.Blue, new List<TeamPlayers>()) { };
        var TeamRed = new Team(TeamTypes.Red, new List<TeamPlayers>()) { };

        foreach (var token in teams.teamA)
        {
            var player = players.First(el => el.Value.AccessToken == token).Value;
            if (player != null) TeamBlue.teamPlayers.Add(new TeamPlayers(player.Connection.connectionId, player.Connection.identity, player.AccessToken));
        }

        foreach (var token in teams.teamB)
        {
            var player = players.First(el => el.Value.AccessToken == token).Value;
            if (player != null) TeamRed.teamPlayers.Add(new TeamPlayers(player.Connection.connectionId, player.Connection.identity, player.AccessToken));
        }
        Teams = new List<Team> { TeamBlue, TeamRed };
        //foreach (var item in players)
        //{
        //    var team = isTeam1 ? TeamTypes.Blue : TeamTypes.Red;
        //    var myTeam = Teams.Where(t => t.teamType == team).FirstOrDefault();
        //    TeamPlayers teamPlayer = new TeamPlayers(item.Value.Connection.connectionId, item.Value.Connection.identity, item.Value.AccessToken);
        //    myTeam.teamPlayers.Add(teamPlayer);
        //    isTeam1 = !isTeam1;
        //}
    }
    public void InitilizeTeamOfThePlayer()
    {
        foreach (var team in Teams)
        {
            foreach (var player in team.teamPlayers)
            {
                if (player.netIdentity.gameObject.TryGetComponent<PlayerSetup>(out PlayerSetup playerSetup))
                {
                    playerSetup.InitilizeTeamOfThisPlayer(team.teamType);
                }

            }
        }
    }


    public bool IsInMyTeam(NetworkIdentity otherPlayer)
    {

        var result3 = from personGroup in Teams
                      from person in personGroup.teamPlayers
                      where person.netIdentity.Equals(NetworkClient.localPlayer)
                      select personGroup;

        var ourTeam = result3.FirstOrDefault();

        var result4 = from personGroup in Teams
                      from person in personGroup.teamPlayers
                      where person.netIdentity.Equals(otherPlayer)
                      select personGroup;
        var otherTeam = result4.FirstOrDefault();

        return ourTeam.teamType == otherTeam.teamType;

    }
    public bool IsInMyTeam(uint otherPlayerNetId)
    {
        var result3 = from personGroup in Teams
                      from person in personGroup.teamPlayers
                      where person.netIdentity.netId.Equals(NetworkClient.localPlayer.netId)
                      select personGroup;

        var ourTeam = result3.FirstOrDefault();


        var result4 = from personGroup in Teams
                      from person in personGroup.teamPlayers
                      where person.netIdentity.netId.Equals(otherPlayerNetId)
                      select personGroup;

        var otherTeam = result4.FirstOrDefault();


        return ourTeam.teamType == otherTeam.teamType;


    }

    public bool IsInMyTeam(uint ownerNetID, uint otherPlayerNetID)
    {
        var myTeam = Teams.Where(_team => _team.teamPlayers.Any(_teamPlayer => _teamPlayer.netIdentity.netId == ownerNetID)).FirstOrDefault();
        var res = myTeam.teamPlayers.Where(_teamPlayer => _teamPlayer.netIdentity.netId == otherPlayerNetID);
        return res.Count() > 0;

        var result3 = from personGroup in Teams
                      from person in personGroup.teamPlayers
                      where person.netIdentity.netId.Equals(ownerNetID)
                      select personGroup;
        //if (result3.ToArray().Length <=0)
        //{
        //    return false;
        //}
        var ourTeam = result3.FirstOrDefault();

        var result2 = from personGroup in Teams
                      from person in personGroup.teamPlayers
                      where person.netIdentity.netId.Equals(otherPlayerNetID)
                      select personGroup;
        var otherTeam = result2.FirstOrDefault();

        //if (result3.ToArray().Length <= 0)
        //{
        //    return false;
        //}

        return ourTeam.teamType == otherTeam.teamType;


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
    public virtual void ServerStarted(Dictionary<int, MatchPeer> players)
    {
        CreateTeam(players);

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


    [Command(requiresAuthority = false)]
    public void CmdReady(string AccessToken)
    {
        Info("Ready! " + AccessToken);

        if (MatchNetworkManager.Instance.numPlayers >= ACGDataManager.Instance.GameData.MaxPlayerCount)
        {
            RpcStartGame();
        }
    }




    #endregion

    #region Team Creation

    #endregion

}


