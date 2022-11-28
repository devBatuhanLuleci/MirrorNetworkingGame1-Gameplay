using kcp2k;
using Mirror;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class NetworkedGameManager : NetworkBehaviour
{
    #region Fields
    private static NetworkedGameManager instance;
    public static NetworkedGameManager Instance
    {
        get
        {
            return instance;
        }
    }
    public GameObject waitingPanel;
    private NetworkIdentity networkIdentity;
    #endregion
    private void Awake()
    {
        instance = this;
        networkIdentity = GetComponent<NetworkIdentity>();
        waitingPanel = GameObject.Find("WaitingPanel");
        Debug.Log("awake: " + MatchNetworkManager.Instance.mode);
    }
    
    private void Start()
    {
        if (!ACGDataManager.Instance.GameData.IsServer)
        {
            ClientStarted();
            CmdReady();
            Debug.Log("awake: " + MatchNetworkManager.Instance.mode);

        }

    }

    public void ClientStarted()
    {
        //var vfxManagerPrefab = Resources.Load<GameObject>("VfxManager");
        //if (vfxManagerPrefab == null)
        //{
        //    Debug.LogError("vfxManagerPrefab is null");
        //}
        //else
        //{
        //    NetworkClient.RegisterPrefab(vfxManagerPrefab.gameObject);
        //}

        string msg = $"Client Started. Port: {ACGDataManager.Instance.GameData.Port}";
        Info(msg);
    }
    public void ServerStarted()
    {

        // Setup();
        string msg = $" <color=green> Server listining on </color> localhost:{ACGDataManager.Instance.GameData.Port}";
        Info(msg);
    }


    private void Setup()
    {
        var vfxManagerPrefab = Resources.Load<GameObject>("VfxManager");
        if (vfxManagerPrefab == null)
        {
            Debug.LogError("vfxManagerPrefab is null");
        }
        else
        {
            var vfxManager = Instantiate(vfxManagerPrefab);
            NetworkServer.Spawn(vfxManager);
        }
    }


    public void StartGame()
    {
        Info("StartGame");
        waitingPanel.SetActive(false);
    }

    public void Info(string msg)
    {
        waitingPanel.GetComponentInChildren<TextMeshProUGUI>().text = msg;
        Debug.LogError(msg);

    }

    #region RPCMethods
    [ClientRpc]
    public void RpcStartGame()
    {
        Debug.LogError("RpcStartGame");
        StartGame();
    }
    #endregion
    #region Command Methods

    public int playerCount = 0;
    [Command(requiresAuthority = false)]
    public void CmdReady()
    {
        Debug.LogError("Ready!");
        playerCount++;
        SetPlayerCount(playerCount);
        if (playerCount > 0)
        {
            RpcStartGame();
        }
    }

    [ClientRpc]
    public void SetPlayerCount(int value)
    {
        this.playerCount = value;
    }
    #endregion

}


