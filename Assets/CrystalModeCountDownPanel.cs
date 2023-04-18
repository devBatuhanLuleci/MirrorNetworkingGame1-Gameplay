using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrystalModeCountDownPanel : CrystalModePanel
{
    public GameObject TeamCountDownInfoText;
    public GameObject CountDownTextObject;

    private TextMeshProUGUI CountDownText;
    private RectTransform CrystalInfoRectTransform;
    private void Awake()
    {
        CountDownText = CountDownTextObject.GetComponent<TextMeshProUGUI>();

        if (TeamCountDownInfoText.TryGetComponent<RectTransform>(out RectTransform rectTransform))
        {

            this.CrystalInfoRectTransform = rectTransform;



        }
    }
    public override void DeactivateOnInit()
    {
        base.DeactivateOnInit();
        ChangeCrystalModeCountDownTeamInfoTextScale(0);
        ChangeCrystalModeCountDownText();
        HandleCrystalInfoPanel(new GameObject[] { TeamCountDownInfoText, CountDownTextObject }, false, true);
    }

    public void ChangeCrystalModeCountDownText(int value)
    {
     //   Debug.Log($"Value: {value}");
        CountDownText.text=value.ToString();
    }
 
    public void HandleCountDownTeamInfoTextPanel(bool isActive)
    {

        HandleCrystalInfoPanel(new GameObject[] { TeamCountDownInfoText/*, CountDownTextObject*/ }, isActive,false,false);

    }
    public void ActivateCountDownTextObject(bool isActive)
    {

        HandleCrystalInfoPanel(new GameObject[] {  CountDownTextObject }, isActive);

    }

    public void ChangeCrystalModeCountDownTeamInfoTextScale(float scaleValue)
    {
        CrystalInfoRectTransform.localScale = new Vector3(scaleValue, scaleValue, 1f);
    }
    public void ChangeCrystalModeCountDownText()
    {
        CountDownText.text ="";
    }
}
