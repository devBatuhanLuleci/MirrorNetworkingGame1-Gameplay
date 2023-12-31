using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MailBoxPanel : Panel
{
    public Button BackButton;
    public Button ChatSendButton;
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
        // ChatSendButton.onClick.AddListener(OnClick_ChatSendButton);
    }
    void RemoveListenerCall()
    {
        BackButton.onClick.RemoveListener(OnClick_BackButton);
        // ChatSendButton.onClick.RemoveListener(OnClick_ChatSendButton);
    }
    #endregion

    #region Buttons
    public void OnClick_BackButton()
    {
        MainPanelUIManager.Instance.BackButton(BackPanel);
    }
    /* public void OnClick_ChatSendButton() mail aktiflesince 
     {
         MainPanelUIManager.Instance.ChatSendButton();
     }*/
    public void OnClick_FriendProfileButton()
    {
        MainPanelUIManager.Instance.FriendProfilePanelShow();
        MainPanelUIManager.Instance.friendProfilePanel.GetComponent<FriendProfilePanel>().isDirectFriendsPanel = false;
    }
    #endregion
}
