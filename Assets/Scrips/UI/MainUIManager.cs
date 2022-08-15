using MoralisUnity.Kits.AuthenticationKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIManager : Singleton<MainUIManager>
{
    [Header("Setup")]
    [SerializeField]
    private Panel loadingPanel;
    [SerializeField]
    private Panel loginPanel;
    [SerializeField]
    private Panel registerPanel;
    [SerializeField]
    private Panel PickCharacterPanel;
    [SerializeField]
    private Panel CharacterNFTMintPanel;

    [Space]
    [SerializeField]
    private Panel startPanel = null;

    private Panel currentPanel;

    #region MonoBehavior Methods



    private void Start()
    {
        Initialize();
        CheckServer();
    }
   

    #endregion

    private void Initialize()
    {
        AddListeners();
        // if statPnael not null set startpanel or set LadingPanel
        currentPanel = startPanel ?? loadingPanel;
        ShowPanel(currentPanel);
    }


    /// <summary>
    /// Check Server update
    /// </summary>
    private void CheckServer()
    {
    }

    public void MoralisLogin()
    {
        ShowPanel(loginPanel);
    }

    public void PickCharacterPanelShow()
    {
        ShowPanel(PickCharacterPanel);
    }

    private void ShowPanel(Panel panel)
    {
        currentPanel.Close();
        panel.Show();
        currentPanel = panel;
    }
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
        ShowPanel(PickCharacterPanel);
    }
    private void OnUserUnregister()
    {
        ShowPanel(registerPanel);
    }
    #endregion

}
