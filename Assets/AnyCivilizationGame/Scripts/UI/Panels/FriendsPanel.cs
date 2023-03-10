using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsPanel : Panel
{
    public Button BackButton;
    public Button FriendAddButton;
    //public Button FriendProfileButton;
    [SerializeField] Panel BackPanel;
    private void Awake()
    {
        BackButton.onClick.AddListener(OnClick_BackButton);
    }
    public void OnClick_BackButton()
    {
        MainPanelUIManager.Instance.BackButton(BackPanel);
    }
    /* public void OnClick_FriendAddButton() veri islemede aktiflesecek
     {
         MainPanelUIManager.Instance.FriendAddButton();
     }*/
    public void OnClick_FriendProfileButton()
    {
        MainPanelUIManager.Instance.FriendProfilePanelShow();
        MainPanelUIManager.Instance.friendProfilePanel.GetComponent<FriendProfilePanel>().isDirectFriendsPanel = true;
    }

}
