using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrystalModeFinishPanel : CrystalModePanel
{
    public GameObject CrystalFinishPanelBG;
    public GameObject WinnerTeamTextObject;

    private TextMeshProUGUI WinnerTeamText;



    private void Awake()
    {
        WinnerTeamText=WinnerTeamTextObject.GetComponent<TextMeshProUGUI>();
    }
    public void ChangeWinnerTeamText(string TeamNameInfo)
    {
        //  SetWinnerTeamCountDownTextColor(TeamNameInfo);
        WinnerTeamText.text = TeamNameInfo + " TEAM WON";
    }
    public override void DeactivateOnInit()
    {
        base.DeactivateOnInit();
        ChangeWinnerTeamText();

        HandleCrystalInfoPanel(new GameObject[] { CrystalFinishPanelBG, WinnerTeamTextObject }, false, true);
    }
    public void ChangeWinnerTeamText()
    {
        WinnerTeamText.text = "";
    }

    public void ActivateCountDownTextObject(bool isActive)
    {

        HandleCrystalInfoPanel(new GameObject[] { CrystalFinishPanelBG,WinnerTeamTextObject }, isActive);

    }

}
