using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClanProfilePanel : Panel
{
    public Button BackButton;
    [SerializeField] Panel BackPanel;
    [SerializeField] public bool isDirectClanPanel = false;
   
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
    }
    void RemoveListenerCall()
    {
        BackButton.onClick.RemoveListener(OnClick_BackButton);
    }
    #endregion

    #region Buttons
    public void OnClick_BackButton()
    {
        if (isDirectClanPanel)
        {
            MainPanelUIManager.Instance.BackButton(BackPanel);
        }
        else
        {
            MainPanelUIManager.Instance.BackButton(MainPanelUIManager.Instance.leaderBoardPanel);
        }

    }
    #endregion
}
