using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguagePanel : Panel
{
    public Button BackButton;
    public Button TurkishButton;
    public Button EnglishButton;
    [SerializeField] Panel BackPanel;
    private void Awake()
    {
        BackButton.onClick.AddListener(OnClick_BackButton);
    }
    public void OnClick_BackButton()
    {
        MainPanelUIManager.Instance.BackButton(BackPanel);
    }
}
