using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClanPanel : Panel
{
    public Button BackButton;
    public Button SearchButton;
    public Button CreateClanButton;
    public Button ClanJoinButton;
    public Button ClanInviteButton;
    [SerializeField] Panel BackPanel;
    [SerializeField] public bool isDirectMainMenu = false;
    private void Awake()
    {
        BackButton.onClick.AddListener(OnClick_BackButton);
        SearchButton.onClick.AddListener(OnClick_SearchButton);
        CreateClanButton.onClick.AddListener(OnClick_CreateClanButton);
        ClanJoinButton.onClick.AddListener(OnClick_ClanJoinButton);
        ClanInviteButton.onClick.AddListener(OnClick_ClanInviteButton);
    }
    public void OnClick_BackButton()
    {
        if (isDirectMainMenu)
        {
            MainPanelUIManager.Instance.BackButton(BackPanel);
        }
        else
        {
            MainPanelUIManager.Instance.BackButton(MainPanelUIManager.Instance.settingMenuPanel);
        }

    }
    public void OnClick_SearchButton()
    {
        // MainPanelUIManager.Instance.BackButton();
    }
    public void OnClick_CreateClanButton()
    {
        // MainPanelUIManager.Instance.BackButton();
    }
    public void OnClick_ClanJoinButton()
    {
        // MainPanelUIManager.Instance.BackButton();
    }
    public void OnClick_ClanInviteButton()
    {
        // MainPanelUIManager.Instance.BackButton();
    }
    public void OnClick_ClanProfileButton()
    {
        MainPanelUIManager.Instance.ClanProfilePanelShow();
        MainPanelUIManager.Instance.clanProfilePanel.GetComponent<ClanProfilePanel>().isDirectClanPanel = true;
    }

}
