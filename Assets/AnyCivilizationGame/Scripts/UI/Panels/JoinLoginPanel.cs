using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinLoginPanel : Panel
{
    public Button JoinButton;
    public Button LoginButton;

    private void Awake()
    {
        JoinButton.onClick.AddListener(OnClick_JoinButton);
        LoginButton.onClick.AddListener(OnClick_LoginButton);
    }
    public void OnClick_JoinButton()
    {
        MainPanelUIManager.Instance.JoinPanelShow();
    }
    public void OnClick_LoginButton()
    {
        MainPanelUIManager.Instance.LoginPanelShow();
    }
}
