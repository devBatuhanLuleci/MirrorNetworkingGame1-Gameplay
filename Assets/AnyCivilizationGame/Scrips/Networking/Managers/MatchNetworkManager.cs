using kcp2k;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;

[System.Serializable]
public class MatchNetworkManager : NetworkManager
{

    #region Fields
    public static MatchNetworkManager Instance { get { return instance; } }

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
        if (ACGDataManager.Instance.GameData.IsServer)
        {
            StartServerNetwork();
        }
        else
        {
            StartClientNetwork();
        }
    }
    private void StartClientNetwork()
    {
        networkAddress = ACGDataManager.Instance.GameData.NetworkAddress;
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

        var prefab = Resources.Load<NetworkedGameManager>(nameof(NetworkedGameManager));
        var networkedGameManager = Instantiate(prefab);
        NetworkServer.Spawn(networkedGameManager.gameObject);
        
        NetworkedGameManager.Instance.ServerStarted();
        players = new Dictionary<int, NetworkConnectionToClient>();

        LoadBalancer.Instance.SpawnServer.SendClientRequestToServer(new OnReadyEvent(ACGDataManager.Instance.GameData.Port));
        NetworkedGameManager.Instance.Info("OnReadyEvent msg sended to master server.");
    }
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        //players.Add(conn.connectionId, conn);
        //NetworkedGameManager.Instance.Info("OnServerConnect players count:" + players.Count);
        //if (players.Count > 2)
        //{
        //    //Invoke("StartGame", 3);
        //}
    }

    public void StartGame()
    {
        NetworkedGameManager.Instance.Info("game is starting...");
        NetworkedGameManager.Instance.RpcStartGame();
    }



}
