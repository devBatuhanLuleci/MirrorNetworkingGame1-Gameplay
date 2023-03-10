using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroidsPanel : Panel
{
    public Button BackButton;
    //public Button DroidButton;
    [SerializeField] Panel BackPanel;
    private void Awake()
    {
        BackButton.onClick.AddListener(OnClick_BackButton);
    }
    public void OnClick_BackButton()
    {
        MainPanelUIManager.Instance.BackButton(BackPanel);
    }
    public void OnClick_DroidButton()
    {
        MainPanelUIManager.Instance.DroidProfilePanelShow();
    }
}
