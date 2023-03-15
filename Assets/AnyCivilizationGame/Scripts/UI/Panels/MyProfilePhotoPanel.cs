using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyProfilePhotoPanel : Panel
{
    public Button BackButton;
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
    }
    void RemoveListenerCall()
    {
        BackButton.onClick.RemoveListener(OnClick_BackButton);
    }
    #endregion

    #region Buttons
    public void OnClick_BackButton()
    {
        MainPanelUIManager.Instance.BackButton(BackPanel);
    }
    public void OnClick_ProfilePhotoButton()
    {

    }
    #endregion
}
