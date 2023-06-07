

using kcp2k;
using Mirror;
using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MatchNetworkManager : NetworkManager
{
    public class MatchPeer
    {
        public NetworkConnectionToClient Connection { get; }
        public string AccessToken = string.Empty;
        public bool IsReady = false;
        public MatchPeer(NetworkConnectionToClient connection)
        {
            Connection = connection;
        }

    }
    #region Fields
    public static MatchNetworkManager Instance { get { return instance; } }

    public OnCharacterReplacedEvent OnPlayerListChanged = new OnCharacterReplacedEvent();


    #endregion

    #region Private Fields

    private static MatchNetworkManager instance;
    private Dictionary<int, MatchPeer> Players;





    #endregion
    public override void Awake()
    {
        base.Awake();
        instance = this;
    }

    public override void Start()
    {
        base.Start();
        if (ACGDataManager.Instance.GameData.TerminalType == TerminalType.Server)
        {
            StartServerNetwork();
        }
        else
        {
            StartClientNetwork();
        }
    }
    #region Game Logich

    /// <summary>
    /// this method must call from server
    /// </summary>
    public void Respawn(PlayerController player)
    {
        StartCoroutine(IERespawn(player));
    }
    public void DestroyThis(ObjectController obj)
    {
        if (obj != null)
        {

            obj.DestroyThisObject();
        }
    }
    /// <summary>
    /// this method must call from server
    /// </summary>
    public IEnumerator IERespawn(PlayerController player)
    {
        yield return new WaitForSeconds(5);
        if (player != null)
        {
            player.Respawn();
        }
    }
    #endregion

    #region  Server Logich
    #region Character creating



    private void RegisterNetworkMessages()
    {
        NetworkServer.RegisterHandler<CharacterCreateMessage>(OnCreateCharacter);
        NetworkServer.RegisterHandler<ReplanceCharacterMessage>(OnReplacePlayer);
        NetworkServer.RegisterHandler<JoinToGameNetworkMessage>(JoinedToGame);
    }

    protected virtual void JoinedToGame(NetworkConnectionToClient conn, JoinToGameNetworkMessage message)
    {
        Debug.Log("JoinedToGame:" + message.AccesToken);
        if (Players.TryGetValue(conn.connectionId, out var player))
        {
            player.AccessToken = message.AccesToken;
            player.IsReady = true;
        }

        if (Players.Count == ACGDataManager.Instance.GameData.MaxPlayerCount
            && NetworkedGameManager.Instance == null
            && !Players.Any(it => !it.Value.IsReady))
        {
            CreateGameManager();
        }
    }

    void OnCreateCharacter(NetworkConnectionToClient conn, CharacterCreateMessage message)
    {
        Debug.LogError("OnCreateCharacter message:" + message.ToString());

        var characterPrefab = spawnPrefabs.Find(el => el.name == message.name);
        // playerPrefab is the one assigned in the inspector in Network
        // Manager but you can use different prefabs per race for example
        GameObject go = Instantiate(characterPrefab);


        // Apply data from the message however appropriate for your game
        // Typically Player would be a component you write with syncvars or properties
        PlayerController player = go.GetComponent<PlayerController>();


        // call this to use this gameobject as the primary controller
        NetworkServer.AddPlayerForConnection(conn, go);
        this.OnPlayerListChanged.Invoke(Players);

    }
    public void OnReplacePlayer(NetworkConnectionToClient conn, ReplanceCharacterMessage message)
    {



        // Cache a reference to the current player object
        GameObject oldPlayer = conn.identity.gameObject;



        Debug.LogError("ReplacePlayer message:" + message.ToString());

        var characterPrefab = spawnPrefabs.Find(el => el.name == message.name);
        // playerPrefab is the one assigned in the inspector in Network
        // Manager but you can use different prefabs per race for example
        GameObject go = Instantiate(characterPrefab);



        // Instantiate the new player object and broadcast to clients
        // Include true for keepAuthority paramater to prevent ownership change
        NetworkServer.ReplacePlayerForConnection(conn, go, true);
        this.OnPlayerListChanged.Invoke(Players);

        // Remove the previous player object that's now been replaced
        // Delay is required to allow replacement to complete.
        Destroy(oldPlayer, 0.1f);
    }
    #endregion

    private void StartClientNetwork()
    {
        networkAddress = ACGDataManager.Instance.GameData.GameServerAddress;
        GetComponent<KcpTransport>().Port = ACGDataManager.Instance.GameData.Port;
        StartClient();
    }
    private void StartServerNetwork()
    {

        if (!TryGetComponent<KcpTransport>(out var transport))
        {
            Debug.LogError("when starting the server KcpTransport is not found!");
            return;
        }
        transport.Port = ACGDataManager.Instance.GameData.Port;
        StartServer();

        Players = new Dictionary<int, MatchPeer>();


        // CreateTeam();
        LoadBalancer.Instance.SpawnServer.SendClientRequestToServer(new OnReadyEvent(ACGDataManager.Instance.GameData.Port));
        Debug.LogError("OnReadyEvent msg sended to master server.");
    }

    private void CreateGameManager()
    {

        // TODO: type must be generic
        var prefab = Resources.Load<CrystalModeNetworkedGameManager>(nameof(CrystalModeNetworkedGameManager));
        var networkedGameManager = Instantiate(prefab);
        NetworkServer.Spawn(networkedGameManager.gameObject);
        Debug.LogError("NetworkedGameManager spawned.");

        NetworkedGameManager.Instance.ServerStarted(Players);

    }


    public PlayerController GetPlayerByNetID(uint netID)
    {

        foreach (var item in Players.Values)
        {
            if (item.Connection.identity.netId == netID)
            {
                Debug.Log($"isim: {item.Connection.identity.name}  id:     {item.Connection.identity.netId}");
                return item.Connection.identity.GetComponent<PlayerController>();
            }
        }
        return null;
    }

    public PlayerController GetPlayerByConnectionID(int connectionId)
    {

        if (Players.TryGetValue(connectionId, out var item))
        {

            return item.Connection.identity.GetComponent<PlayerController>();

        }


        return null;
    }

    public PlayerController GetPlayerByConnection(NetworkConnectionToClient conn)
    {
        // Debug.Log(conn.);

        //foreach (var item in players.Values)
        //{
        //    if (item.identity.netId == netID)
        //    {
        //        //  Debug.Log($"isim: { item.identity.name}  id:     {item.identity.netId}");
        //        return item.identity.GetComponent<PlayerController>();
        //    }
        //}
        //if(conn.identity.TryGetComponent<PlayerController>(out PlayerController playerController))
        //{
        //    return playerController;
        //}

        return null;
    }


    #endregion


    #region Mirror Overrides

    public override void OnStartServer()
    {
        base.OnStartServer();
        RegisterNetworkMessages();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {

        Players.Remove(conn.connectionId);
        OnPlayerListChanged.Invoke(Players);
        base.OnServerDisconnect(conn);
    }

    public override void OnClientConnect()
    {


        base.OnClientConnect();
        // you can send the message here, or wherever else you want
        var defaultCharacter = CharacterCreateMessage.Default;
        var joinMsg = new JoinToGameNetworkMessage { AccesToken = AuthenticationManager.Instance.User.accessToken };
        NetworkClient.Send(joinMsg);
        NetworkClient.Send(defaultCharacter);
    }
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {

        base.OnServerConnect(conn);
        var playerPeer = new MatchPeer(conn);
        Players.Add(conn.connectionId, playerPeer);
        OnPlayerListChanged.Invoke(Players);
    }
    #endregion
}
[System.Serializable]
public class OnCharacterReplacedEvent : UnityEvent<Dictionary<int, MatchNetworkManager.MatchPeer>>
{

}

