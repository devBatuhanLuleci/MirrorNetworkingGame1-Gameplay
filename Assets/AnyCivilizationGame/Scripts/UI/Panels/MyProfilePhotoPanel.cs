using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyProfilePhotoPanel : Panel
{
    public Button BackButton;
    [SerializeField]Panel BackPanel;
    private void Awake()
    {
        BackButton.onClick.AddListener(OnClick_BackButton);
    }
    public void OnClick_BackButton()
    {
        MainPanelUIManager.Instance.BackButton(BackPanel);
    }
    public void OnClick_ProfilePhotoButton()
    {

    }
}
