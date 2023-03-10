using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomCodeOnlineFriendsPanel : Panel
{
    public Button BackButton;
    // public Button FriendProfileButton;
    //public Button InviteButton;
    [SerializeField]Panel BackPanel;
    private void Awake()
    {
        BackButton.onClick.AddListener(OnClick_BackButton);
    }
    public void OnClick_BackButton()
    {
        MainPanelUIManager.Instance.BackButton(BackPanel);
    }
    public void OnClick_FriendProfileButton()
    {
        MainPanelUIManager.Instance.FriendProfilePanelShow();
    }
    public void OnClick_Invite()
    {
        //  MainPanelUIManager.Instance.InviteButton();
    }
}
