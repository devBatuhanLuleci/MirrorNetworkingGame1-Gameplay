using MoralisUnity.Kits.AuthenticationKit;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

class MainUIManager : Panel
{
    [Header("Setup")]
    public Panel loadingPanel;
    public Panel loginPanel;
    public Panel registerPanel;
    public Panel PickCharacterPanel;
    public Panel CharacterNFTMintPanel;
    public Panel lobbyPanel;
    public Panel mainMenuPanel;
    [Space]
    [SerializeField]
    public Panel startPanel = null;

    public static MainUIManager Instance
    {
        get;   // get method
        private set;
    }


    #region MonoBehavior Methods

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Initialize();
        CheckServer();
    }


    #endregion
    #region Methods

    private void Initialize()
    {
        AddListeners();
        // if statPnael not null set startpanel else set LadingPanel
        var currentPanel = startPanel ?? loadingPanel;
        Show(currentPanel);
       
    }   



    public void MoralisLogin()
    {
        Show(loginPanel);
    }

    public void PickCharacterPanelShow()
    {
        Show(PickCharacterPanel);
    }


    /// <summary>
    /// Check Server update
    /// </summary>
    private void CheckServer()
    {
    }


    #endregion

    #region Listeners

    private void AddListeners()
    {
        AuthenticationKit.Instance.OnConnected.AddListener(OnConnected);
        AuthenticationManager.Instance.OnUserUnregister.AddListener(OnUserUnregister);
        AuthenticationManager.Instance.OnUserLogged.AddListener(OnUserLogged);

    }
    private void RemoveListeners()
    {
        AuthenticationKit.Instance.OnConnected.RemoveListener(OnConnected);
        AuthenticationManager.Instance.OnUserUnregister.RemoveListener(OnUserUnregister);
        AuthenticationManager.Instance.OnUserLogged.RemoveListener(OnUserLogged);
    }

    #endregion

    #region Moralis Event Handler
    private void OnConnected()
    {
        loginPanel.Close();
        Debug.Log("User successfully logged so time to connect to the game server. :)");
    }
    #endregion

    #region Authentication Event Handler

    private void OnUserLogged()
    {
        // TODO check user have any nft character 
        // ?f not  open Nft buy panel
        Show(lobbyPanel);

    }
    private void OnUserUnregister()
    {
        Show(registerPanel);
    }

    internal void Login()
    {

        var loginType = AuthenticationManager.Instance.LoginType;
        var isServer = AuthenticationManager.Instance.IsServer;
        if (isServer) return;
        switch (loginType)
        {
            case LoginType.Moralis:
                MoralisLogin();
                break;
            case LoginType.User:
                Debug.Log("login with custom user");
                break;
            case LoginType.Admin:
                Debug.Log("login with Admin");
                break;
            case LoginType.None:
                Show(lobbyPanel);
                break;
            default:
                break;
        }
    }
    #endregion

}
