using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomCodePanel : Panel
{
    public Button BackButton;
   // public Button PlusButton;
   // public Button DroidChooseButton;
    public Button ReadyButton;
    [SerializeField]Panel BackPanel;
    private void Awake()
    {
        BackButton.onClick.AddListener(OnClick_BackButton);
        ReadyButton.onClick.AddListener(OnClick_ReadyButton);
    }
    public void OnClick_BackButton()
    {
        MainPanelUIManager.Instance.BackButton(BackPanel);
    }
    public void OnClick_PlusButton()
    {
        MainPanelUIManager.Instance.RoomCodeOnlineFriendsPanel();
    }
    public void OnClick_DroidChooseButton()
    {
        MainPanelUIManager.Instance.RoomCodeDroidChoosePanel();
    }
    public void OnClick_ReadyButton()
    {

    }

}
