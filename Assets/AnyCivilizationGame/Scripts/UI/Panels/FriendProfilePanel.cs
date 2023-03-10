using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendProfilePanel : Panel
{
    public Button BackButton;
    //public Button FriendLastMatchButton;
    [SerializeField] Panel BackPanel;
    [SerializeField] public bool isDirectFriendsPanel=false;
    private void Awake()
    {
        BackButton.onClick.AddListener(OnClick_BackButton);
    }
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
}
