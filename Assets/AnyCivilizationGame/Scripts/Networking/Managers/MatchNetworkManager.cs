

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

    #region Fields
    public static MatchNetworkManager Instance { get { return instance; } }
        
    public OnCharacterReplacedEvent OnPlayerListChanged = new OnCharacterReplacedEvent();
   

    #endregion

    #region Private Fields

    private static MatchNetworkManager instance;
    private Dictionary<int, NetworkConnectionToClient> players;


   
   

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

    public override void OnStartServer()
    {
        base.OnStartServer();
        RegisterNetworkMessages();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {

        players.Remove(conn.connectionId);
        OnPlayerListChanged.Invoke(players);
        base.OnServerDisconnect(conn);
    }


    private void RegisterNetworkMessages()
    {
        NetworkServer.RegisterHandler<CharacterCreateMessage>(OnCreateCharacter);
        NetworkServer.RegisterHandler<ReplanceCharacterMessage>(OnReplacePlayer);
    }

    public override void OnClientConnect()
    {


        base.OnClientConnect();
        // you can send the message here, or wherever else you want
        var defaultCharacter = CharacterCreateMessage.Default;
        NetworkClient.Send(defaultCharacter);
    }

    void OnCreateCharacter(NetworkConnectionToClient conn, CharacterCreateMessage message)
    {
        Debug.LogError("OnCreateCharacter message:" + message.ToString());

        var characterPrefab = spawnPrefabs.Find(el => el.name == message.name);
        // playerPrefab is the one assigned in the inspector in Network
        // Manager but you can use different prefabs per race for example
        GameObject go = Instantiate(characterPrefab);


    //    GetIntoTeam(go, players.Count % 2);


        // Apply data from the message however appropriate for your game
        // Typically Player would be a component you write with syncvars or properties
        PlayerController player = go.GetComponent<PlayerController>();


        // call this to use this gameobject as the primary controller
        NetworkServer.AddPlayerForConnection(conn, go);
        this.OnPlayerListChanged.Invoke(players);

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
        this.OnPlayerListChanged.Invoke( players);

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

        players = new Dictionary<int, NetworkConnectionToClient>();


       // CreateTeam();
        LoadBalancer.Instance.SpawnServer.SendClientRequestToServer(new OnReadyEvent(ACGDataManager.Instance.GameData.Port));
        Debug.LogError("OnReadyEvent msg sended to master server.");
    }

    private void CreateGameManager()
    {
       

        var prefab = Resources.Load<CrystalModeNetworkedGameManager>(nameof(CrystalModeNetworkedGameManager));
        var networkedGameManager = Instantiate(prefab);
        NetworkServer.Spawn(networkedGameManager.gameObject);
        Debug.LogError("NetworkedGameManager spawned.");

        NetworkedGameManager.Instance.ServerStarted(players);

    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {

        base.OnServerConnect(conn);
        players.Add(conn.connectionId, conn);
        OnPlayerListChanged.Invoke(players);

        Debug.LogError("OnServerConnect players count:" + players.Count);
        if (players.Count >= ACGDataManager.Instance.GameData.MaxPlayerCount && NetworkedGameManager.Instance == null)
        {
            //Invoke("StartGame", 1);
         Invoke("CreateGameManager",2f);

        }
    }
  
    public PlayerController GetPlayerByNetID(uint netID)
    {

        foreach (var item in players.Values)
        {
            if (item.identity.netId == netID)
            {
                 Debug.Log($"isim: { item.identity.name}  id:     {item.identity.netId}");
                return item.identity.GetComponent<PlayerController>();
            }
        }
        return null;
    }

    public PlayerController GetPlayerByConnectionID(int connectionId)
    {

        if (players.TryGetValue(connectionId, out var item))
        {
           
            return item.identity.GetComponent<PlayerController>();

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


    #endregion
}
[System.Serializable]
public class OnCharacterReplacedEvent : UnityEvent< Dictionary<int, NetworkConnectionToClient>>
{

}

