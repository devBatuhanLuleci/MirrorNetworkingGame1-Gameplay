using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyProfilePanel : Panel
{
    public Button BackButton;
    public Button MyProfilePhotoButton;
    [SerializeField]Panel BackPanel;
    private void Awake()
    {
        BackButton.onClick.AddListener(OnClick_BackButton);
        MyProfilePhotoButton.onClick.AddListener(OnClick_MyProfilePhotoButton);
    }
    public void OnClick_BackButton()
    {
        MainPanelUIManager.Instance.BackButton(BackPanel);
    }
    public void OnClick_MyProfilePhotoButton()
    {
        MainPanelUIManager.Instance.MyProfilePhotoPanelShow();
    }
    public void OnClick_LastMachButton()
    {
        MainPanelUIManager.Instance.LastMatchPanelShow();
    }


}
