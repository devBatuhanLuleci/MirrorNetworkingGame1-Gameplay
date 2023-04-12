using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendProfilePanel : Panel
{
    public Button BackButton;
    //public Button FriendLastMatchButton;
    [SerializeField] Panel BackPanel;
    [SerializeField] public bool isDirectFriendsPanel = false;

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
        if (isDirectFriendsPanel)
        {
            MainPanelUIManager.Instance.BackButton(BackPanel);
        }
        else
        {
            MainPanelUIManager.Instance.BackButton(MainPanelUIManager.Instance.mailBoxPanel);
        }

    }
    public void OnClick_FriendLastMachButton()
    {
        MainPanelUIManager.Instance.FriendLastMatchPanelShow();
    }
    #endregion
}
