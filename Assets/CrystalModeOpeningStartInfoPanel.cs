using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;
using Mirror;
using System;

public class CrystalModeOpeningStartInfoPanel : CrystalModePanel
{
    public GameObject CrystalInfoTexts;
    public GameObject CrystalStartInfoBG;
  
    private RectTransform CrystalInfoRectTransform;

   
    private void Awake()
    {
        CrystalInfoRectTransform = CrystalInfoTexts.GetComponent<RectTransform>();

    }

    public void Animate_CrystalInfoPanel(bool isActive)
    {

        HandleCrystalInfoPanel(new GameObject[] { CrystalInfoTexts, CrystalStartInfoBG },isActive);

    }
    public override void Init()
    {
        base.Init();
    }
    public override void DeactivateOnInit()
    {
        base.DeactivateOnInit();
        //TODO : bunu crystalinfopanel'de değilde  CrystalModeGamePlayCanvasUIController 'de yap.
        ChangeCrystalInfoRectTransformScale(0);

        HandleCrystalInfoPanel(new GameObject[] { CrystalInfoTexts, CrystalStartInfoBG }, false, true);


    }
   
    public void ChangeCrystalInfoRectTransformScale(float scaleValue)
    {

        CrystalInfoRectTransform.localScale = new Vector3(scaleValue, scaleValue, 1f);

    }


    protected override void OnPanelClose()
    {
        base.OnPanelClose();

        CrystalInfoTexts.SetActive(false);
        CrystalStartInfoBG.SetActive(false);

    }



}
