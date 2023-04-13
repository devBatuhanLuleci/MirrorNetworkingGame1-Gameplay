using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalStatsUIPanelManager : Panel
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



}
