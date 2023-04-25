using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalModeStringSync : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnWinnableTeamCountDownTextChanged))]
    public string WinnableTeamCountDownText;

    [SyncVar(hook = nameof(OnWinnerTeamTextChanged))]
    public string WinnerTeamText;

    public void  OnWinnableTeamCountDownTextChanged(string oldValue,string newValue)
    {
        GameplayPanelUIManager.Instance.SetWinnableTeamCountDownText_1(newValue);

    }
    public void ChangeWinnableTeamCountDownText(string text)
    {
        WinnableTeamCountDownText=text;
    }



    public void OnWinnerTeamTextChanged(string oldValue, string newValue)
    {
        GameplayPanelUIManager.Instance.SetWinnerTeamText(newValue);

    }
    public void ChangeWinnerTeamText(string text)
    {
        WinnerTeamText = text;
    }


}
