using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePanel : Panel
{
    public Button BackButton;
    public Button VideoButton;
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
        VideoButton.onClick.AddListener(OnClick_VideoButton);
    }
    void RemoveListenerCall()
    {
        BackButton.onClick.RemoveListener(OnClick_BackButton);
        VideoButton.onClick.RemoveListener(OnClick_VideoButton);
    }
    #endregion

    #region Buttons
    public void OnClick_BackButton()
    {
        MainPanelUIManager.Instance.BackButton(BackPanel);
    }
    public void OnClick_VideoButton()
    {
        //MainPanelUIManager.Instance.BackButton();
    }
    #endregion
}
