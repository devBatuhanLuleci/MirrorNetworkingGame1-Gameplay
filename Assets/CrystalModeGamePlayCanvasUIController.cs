using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;
using System;
using static UnityEngine.UI.CanvasScaler;

public class CrystalModeGamePlayCanvasUIController : Panel
{

    //private RectTransform GemRectTransform;






    public CrystalStatsUIPanelManager crystalStatsUIPanel;

    public CrystalModeOpeningStartInfoPanel crystalStartInfoPanel;

    public CrystalModeCountDownPanel crystalCountDownPanel;

    public void ActivateCrystalnfoPanel(bool isActive)
    {


        crystalStartInfoPanel.Animate_CrystalInfoPanel(isActive);


    }
    public void ActivateCountDownTeamInfoTextPanel(bool isActive)
    {


        crystalCountDownPanel.HandleCountDownTeamInfoTextPanel(isActive);


    }
    public void ActivateCountDownTextPanel(bool isActive)
    {


        crystalCountDownPanel.ActivateCountDownTextObject(isActive);


    }
    public void Init()
    {

        crystalStartInfoPanel.Init();
        crystalStatsUIPanel.Init();
        crystalCountDownPanel.Init();


    }
    public void HandleCrystalInfoText(float value)
    {

        crystalStartInfoPanel.ChangeCrystalInfoRectTransformScale(value);
    }
    public void HandleCrystalInfoPanelPos(Vector2 value)
    {
        crystalStatsUIPanel.ChangeCrystalStatsRectTransformPos(value);

    }
    public void HandleCrystalModeGameCountdownValue(int value)
    {
        crystalCountDownPanel.ChangeCrystalModeCountDownText(value);

    }
    public void HandleCrystalModeGameTempCountdownValue(int value)
    {
        crystalCountDownPanel.ChangeCrystalModeCountDownText(value);

    }
    public void HandleCrystalModeCountDownTeamInfoTextScale(float value)
    {
        crystalCountDownPanel.ChangeCrystalModeCountDownTeamInfoTextScale(value);

    }
    public void HandleWinnerTeamCountDownText(string TeamNameInfo)
    {
        crystalCountDownPanel.HandleWinnerTeamCountDownText(TeamNameInfo);

    }

    public void HandleCrystalModeGameTime(int value)
    {

        crystalStatsUIPanel.ChangeTimeOnPanel(value);
    }

    public override void Show()
    {

        //   Activate();


    }





}
