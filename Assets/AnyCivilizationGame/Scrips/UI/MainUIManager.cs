using MoralisUnity.Kits.AuthenticationKit;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MainUIManager : Singleton<MainUIManager>
{
    [Header("Setup")]
    public Panel loadingPanel;
    public Panel loginPanel;
    public Panel registerPanel;
    public Panel PickCharacterPanel;
    public Panel CharacterNFTMintPanel;
    public Panel lobbyPanel;

    [Space]
    [SerializeField]
    public Panel startPanel = null;


    private Panel currentPanel;

    #region MonoBehavior Methods



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
        currentPanel = startPanel ?? loadingPanel;
        ShowPanel(currentPanel);
    }



    public void MoralisLogin()
    {
        ShowPanel(loginPanel);
    }

    public void PickCharacterPanelShow()
    {
        ShowPanel(PickCharacterPanel);
    }

    public T GetPanel<T>() where T : Panel
    {
        var props = this.GetType().GetFields();
        for (int i = 0; i < props.Length; i++)
        {
            var item = props[i].GetValue(this);
            if (item is T) return item as T;

        }
        return null;
    }
    /// <summary>
    /// Check Server update
    /// </summary>
    private void CheckServer()
    {
    }
    private void ShowPanel(Panel panel)
    {
        currentPanel.Close();
        panel.Show();
        currentPanel = panel;
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
        ShowPanel(lobbyPanel);

    }
    private void OnUserUnregister()
    {
        ShowPanel(registerPanel);
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
                ShowPanel(lobbyPanel);
                break;
            default:
                break;
        }
    }
    #endregion

}
