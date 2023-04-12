using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenuPanel : Panel
{
    public Button BackButton;
    public Button MyProfileButton;
    public Button SettingsButton;
    public Button FriendsButton;
    public Button MailboxButton;
    public Button ClanButton;
    public Button LeaderBoardButton;
    [SerializeField] Panel BackPanel;


    private void OnEnable()
    {
        AddListenerCall();
    }
    private void OnDisable()
    {
        RemoveListenerCall();
    }

    #region Listeners
    void AddListenerCall()
    {
        BackButton.onClick.AddListener(OnClick_BackButton);
        MyProfileButton.onClick.AddListener(OnClick_MyProfileButton);
        SettingsButton.onClick.AddListener(OnClick_SettingsButton);
        FriendsButton.onClick.AddListener(OnClick_FriendsButton);
        MailboxButton.onClick.AddListener(OnClick_MailBoxButton);
        ClanButton.onClick.AddListener(OnClick_ClanButton);
        LeaderBoardButton.onClick.AddListener(OnClick_LeaderBoardButton);
    }
    void RemoveListenerCall()
    {
        BackButton.onClick.RemoveListener(OnClick_BackButton);
        MyProfileButton.onClick.RemoveListener(OnClick_MyProfileButton);
        SettingsButton.onClick.RemoveListener(OnClick_SettingsButton);
        FriendsButton.onClick.RemoveListener(OnClick_FriendsButton);
        MailboxButton.onClick.RemoveListener(OnClick_MailBoxButton);
        ClanButton.onClick.RemoveListener(OnClick_ClanButton);
        LeaderBoardButton.onClick.RemoveListener(OnClick_LeaderBoardButton);
    }
    #endregion

    #region Buttons
    public void OnClick_BackButton()
    {
        MainPanelUIManager.Instance.BackButton(BackPanel);
    }
    public void OnClick_MyProfileButton()
    {
        MainPanelUIManager.Instance.MyProfilePanelShow();
        MainPanelUIManager.Instance.myProfilePanel.GetComponent<MyProfilePanel>().isDirectMyProfilePanel = false;
    }
    public void OnClick_SettingsButton()
    {
        MainPanelUIManager.Instance.SettingsPanelShow();
    }
    public void OnClick_FriendsButton()
    {
        MainPanelUIManager.Instance.FriendsPanelShow();
    }
    public void OnClick_MailBoxButton()
    {
        MainPanelUIManager.Instance.MailBoxPanelShow();
    }
    public void OnClick_ClanButton()
    {
        MainPanelUIManager.Instance.ClanPanelShow();
        MainPanelUIManager.Instance.clanPanel.GetComponent<ClanPanel>().isDirectMainMenu = false;
    }
    public void OnClick_LeaderBoardButton()
    {
        MainPanelUIManager.Instance.LeaderBoardPanelShow();
    }
    #endregion
}
