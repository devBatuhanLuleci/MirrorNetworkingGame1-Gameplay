using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinLoginPanel : Panel
{
    public Button JoinButton;
    public Button LoginButton;

    #region Listeners
    private void OnEnable()
    {
        AddListenerCall();
    }
    void AddListenerCall()
    {
        JoinButton.onClick.AddListener(OnClick_JoinButton);
        LoginButton.onClick.AddListener(OnClick_LoginButton);
    }
    private void OnDisable()
    {
        RemoveListenerCall();
    }
    void RemoveListenerCall()
    {
        JoinButton.onClick.AddListener(OnClick_JoinButton);
        LoginButton.onClick.AddListener(OnClick_LoginButton);
    }
    #endregion

    #region Buttons
    public void OnClick_JoinButton()
    {
        MainPanelUIManager.Instance.JoinPanelShow();
    }
    public void OnClick_LoginButton()
    {
        MainPanelUIManager.Instance.LoginPanelShow();
    }
    #endregion
}
