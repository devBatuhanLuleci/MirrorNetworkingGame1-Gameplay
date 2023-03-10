using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardPanel : Panel
{
    public Button BackButton;
    public Button GlobalButton;
    public Button LocalButton;
    public Button ClanButton;
    public Button ClanProfileButton;
    [SerializeField] Panel BackPanel;
    private void Awake()
    {
        BackButton.onClick.AddListener(OnClick_BackButton);
        // GlobalButton.onClick.AddListener(OnClick_GlobalButton);
        //  LocalButton.onClick.AddListener(OnClick_LocalButton);
        // ClanButton.onClick.AddListener(OnClick_ClanButton);
        
    }
    public void OnClick_BackButton()
    {
        MainPanelUIManager.Instance.BackButton(BackPanel);
    }
    /*   public void OnClick_GlobalButton()    // Veri cekerken etkinlesecekler
       {
           MainPanelUIManager.Instance.GlobalButton();
       }
       public void OnClick_LocalButton()
       {
           MainPanelUIManager.Instance.LocalButton();
       }
       public void OnClick_ClanButton()
       {
           MainPanelUIManager.Instance.ClanButton();
       }*/
    public void OnClick_ClanProfileButton()
    {
        MainPanelUIManager.Instance.ClanProfilePanelShow();
        MainPanelUIManager.Instance.clanProfilePanel.GetComponent<ClanProfilePanel>().isDirectClanPanel = false;
    }
}
