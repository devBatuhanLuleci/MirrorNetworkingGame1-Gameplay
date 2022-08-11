using MoralisUnity.Kits.AuthenticationKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : Singleton<MainUIManager>
{
    [Header("Setup")]
    [SerializeField]
    private Panel LoadingPanel;
    [SerializeField]
    private Panel LoginPanel;
    [SerializeField]
    private Panel RegisterPanel;



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
        AuthenticationKit.Instance.OnConnected.AddListener(OnConnected);
        AuthenticationManager.Instance.OnUserUnregister.AddListener(OnUserUnregister);
        currentPanel = LoadingPanel;
    }

    /// <summary>
    /// Check Server update
    /// </summary>
    private void CheckServer()
    {
        StartCoroutine(FakeWait(1, MoralisLogin));
    }

    private void MoralisLogin()
    {


        ShowPanel(LoginPanel);
    }

    #region FakeArea
    IEnumerator FakeWait(float time, Action callBack)
    {
        yield return new WaitForSeconds(1);
        if (callBack != null)
            callBack();
    }
    #endregion


    #region Moralis Event Handler
    private void OnConnected()
    {
        LoginPanel.Close();
        Debug.Log("User successfully logged so time to connect to the game server. :)");
    }
    #endregion

    #region Authentication Event Handler
    private void OnUserUnregister()
    {
        ShowPanel(RegisterPanel);
    }
    #endregion

    private void ShowPanel(Panel panel)
    {
        currentPanel.Close();
        panel.Show();
        currentPanel = panel;
    }
}
