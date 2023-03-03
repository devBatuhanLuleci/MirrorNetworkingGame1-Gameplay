using kcp2k;
using Mirror;
using System;
using System.Collections;
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

    public void ClientStarted()
    {

        string msg = $"Client Started. Port: {ACGDataManager.Instance.GameData.Port}";
        Info(msg);
    }
    public void ServerStarted()
    {

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

}


