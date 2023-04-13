using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;
using Mirror;

public class CrystalStartInfoPanel : Panel
{
    public GameObject CrystalInfoTexts;
    public GameObject CrystalStartInfoBG;

    private RectTransform CrystalInfoRectTransform;
    public UnityAction animAction;


    private void Awake()
    {
        CrystalInfoRectTransform = CrystalInfoTexts.GetComponent<RectTransform>();

    }

    public void Animate_CrystalInfoText(bool isActive)
    {

        HandleCrystalInfoPanel(isActive);

    }
    public void Init()
    {
        DeactivateOnInit();
    }
    public void DeactivateOnInit()
    {
        ChangeCrystalInfoRectTransformScale(0);

        HandleCrystalInfoPanel(false);


    }
    public void HandleCrystalInfoPanel(bool activate)
    {

        CrystalInfoTexts.SetActive(activate);
        CrystalStartInfoBG.SetActive(activate);


    }
    public void ChangeCrystalInfoRectTransformScale(float scaleValue)
    {

        CrystalInfoRectTransform.localScale = new Vector3(scaleValue, scaleValue, 1f);

    }

    private void OnAnimationFinished()
    {
        CloseSmoothly();
        animAction.Invoke();
    }



}
