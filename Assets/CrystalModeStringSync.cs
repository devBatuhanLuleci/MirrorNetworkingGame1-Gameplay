using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalModeStringSync : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnWinnerTeamCountDownTextChanged))]
    public string WinnerTeamCountDownText;

    public void  OnWinnerTeamCountDownTextChanged(string oldValue,string newValue)
    {
        GameplayPanelUIManager.Instance.SetWinnerTeamCountDownText_1(newValue);

    }
    public void ChangeWinnerTeamCountDownText(string text)
    {
        WinnerTeamCountDownText=text;
    }
}
