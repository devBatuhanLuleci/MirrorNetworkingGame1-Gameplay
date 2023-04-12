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
        // GlobalButton.onClick.AddListener(OnClick_GlobalButton);
        //  LocalButton.onClick.AddListener(OnClick_LocalButton);
        // ClanButton.onClick.AddListener(OnClick_ClanButton);
    }
    void RemoveListenerCall()
    {
        BackButton.onClick.RemoveListener(OnClick_BackButton);
        // GlobalButton.onClick.RemoveListener(OnClick_GlobalButton);
        //  LocalButton.onClick.RemoveListener(OnClick_LocalButton);
        // ClanButton.onClick.RemoveListener(OnClick_ClanButton);
    }
    #endregion

    #region Buttons
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
    #endregion
}
