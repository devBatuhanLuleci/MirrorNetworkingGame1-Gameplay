using MoralisUnity.Kits.AuthenticationKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : Singleton<MainUIManager>
{
    [Header("Setup")]
    [SerializeField]
    private Panel loadingPanel;
    [SerializeField]
    private Panel loginPanel;
    [SerializeField]
    private Panel registerPanel;

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
    private void OnDestroy()
    {
        AuthenticationKit.Instance.OnConnected.RemoveListener(OnConnected);
        AuthenticationManager.Instance.OnUserUnregister.RemoveListener(OnUserUnregister);
    }
    #endregion

    private void Initialize()
    {
        // if statPnael not null set startpanel or set LadingPanel
        currentPanel = startPanel ?? loadingPanel;
        AuthenticationKit.Instance.OnConnected.AddListener(OnConnected);
        AuthenticationManager.Instance.OnUserUnregister.AddListener(OnUserUnregister);
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



    #region Moralis Event Handler
    private void OnConnected()
    {
        loginPanel.Close();
        Debug.Log("User successfully logged so time to connect to the game server. :)");
    }
    #endregion

    #region Authentication Event Handler
    private void OnUserUnregister()
    {
        ShowPanel(registerPanel);
    }
    #endregion

    private void ShowPanel(Panel panel)
    {
        currentPanel.Close();
        panel.Show();
        currentPanel = panel;
    }
}
