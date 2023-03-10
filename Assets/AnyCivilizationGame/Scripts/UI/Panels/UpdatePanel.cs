using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePanel : Panel
{
    public Button BackButton;
    public Button VideoButton;
    [SerializeField]Panel BackPanel;
    private void Awake()
    {
        BackButton.onClick.AddListener(OnClick_BackButton);
        VideoButton.onClick.AddListener(OnClick_VideoButton);
    }
    public void OnClick_BackButton()
    {
        MainPanelUIManager.Instance.BackButton(BackPanel);
    }
    public void OnClick_VideoButton()
    {
        //MainPanelUIManager.Instance.BackButton();
    }
}
