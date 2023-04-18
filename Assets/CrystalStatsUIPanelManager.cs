using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalStatsUIPanelManager : CrystalModePanel
{
    public AllyTeamUIPanelHandler AllyTeamPanel;
    public EnemyTeamUIPanelHandler EnemyTeamPanel;
    public TimePanelHandler TimePanel;

    private RectTransform rectTransform;


    private void Awake()
    {
        if (TryGetComponent<RectTransform>(out RectTransform rectTransform))
        {

            this.rectTransform = rectTransform;

        }
    }
    public void ChangeCrystalStatsRectTransformPos(Vector2 posValue)
    {

        rectTransform.anchoredPosition = posValue;

    }

    public void ChangeTimeOnPanel(int CountDownTime)
    {
        TimePanel.ChangeTimeText(CountDownTime);
    }

    public override void Init()
    {
       base.Init();
    }
    public  override void DeactivateOnInit()
    {
        base.DeactivateOnInit();

        AllyTeamPanel.ResetCrystalAmountBar();
        EnemyTeamPanel.ResetCrystalAmountBar();
        ChangeCrystalStatsRectTransformPos(new Vector2(0, 50));



    }
}
