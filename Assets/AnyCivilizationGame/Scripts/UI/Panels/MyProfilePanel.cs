using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyProfilePanel : Panel
{
    public Button BackButton;
    public Button MyProfilePhotoButton;
    [SerializeField] Panel BackPanel;
    [SerializeField] public bool isDirectMyProfilePanel = false;

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
        MyProfilePhotoButton.onClick.AddListener(OnClick_MyProfilePhotoButton);
    }
    void RemoveListenerCall()
    {
        BackButton.onClick.RemoveListener(OnClick_BackButton);
        MyProfilePhotoButton.onClick.RemoveListener(OnClick_MyProfilePhotoButton);
    }
    #endregion

    #region Buttons
    public void OnClick_BackButton()
    {
        MainPanelUIManager.Instance.BackButton(BackPanel);

        if (isDirectMyProfilePanel)
        {
            MainPanelUIManager.Instance.BackButton(MainPanelUIManager.Instance.mainMenuPanel);
        }
        else
        {
            MainPanelUIManager.Instance.BackButton(BackPanel);
        }
    }
    public void OnClick_MyProfilePhotoButton()
    {
        MainPanelUIManager.Instance.MyProfilePhotoPanelShow();
    }
    public void OnClick_LastMachButton()
    {
        MainPanelUIManager.Instance.LastMatchPanelShow();
    }

    #endregion
}
