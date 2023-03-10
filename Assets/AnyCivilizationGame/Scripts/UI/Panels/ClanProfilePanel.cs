using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClanProfilePanel : Panel
{
    public Button BackButton;
    [SerializeField] Panel BackPanel;
    [SerializeField] public bool isDirectClanPanel = false;
    private void Awake()
    {
        BackButton.onClick.AddListener(OnClick_BackButton);
    }
    public void OnClick_BackButton()
    {
        if (isDirectClanPanel)
        {
            MainPanelUIManager.Instance.BackButton(BackPanel);
        }
        else
        {
            MainPanelUIManager.Instance.BackButton(MainPanelUIManager.Instance.leaderBoardPanel);
        }

    }
}
