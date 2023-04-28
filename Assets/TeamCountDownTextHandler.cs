using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamCountDownTextHandler : MonoBehaviour
{
    public TextMeshProUGUI WinnerTeamCountDownText_1;
    private Color WinnerTeamCountDownText_1_Color;
    public Color WinnerTeamCountDownText_1_Team1_Color;
    public Color WinnerTeamCountDownText_1_Team2_Color;

    private void Awake()
    {
        WinnerTeamCountDownText_1_Color = WinnerTeamCountDownText_1.color;
    }
    public void SetWinnerTeamCountDownTextColor(string TeamNameInfo)
    {
        //if (teamName == "Team1")
        //{
        //    WinnerTeamCountDownText_1_Color = WinnerTeamCountDownText_1_Team1_Color;

        //}
        //else if( teamName == "Team2")
        //{
        //    WinnerTeamCountDownText_1_Color = WinnerTeamCountDownText_1_Team2_Color;

        //}

        WinnerTeamCountDownText_1_Color = TeamNameInfo ==NetworkedGameManager.TeamTypes.Blue.ToString() ? 
                                          WinnerTeamCountDownText_1_Team1_Color : WinnerTeamCountDownText_1_Team2_Color;

        WinnerTeamCountDownText_1.color = WinnerTeamCountDownText_1_Color;
    }
    public void ChangeWinnerTeamCountDownText_1(string TeamNameInfo)
    {
        SetWinnerTeamCountDownTextColor(TeamNameInfo);
        WinnerTeamCountDownText_1.text = TeamNameInfo + " TEAM";
    }


}
