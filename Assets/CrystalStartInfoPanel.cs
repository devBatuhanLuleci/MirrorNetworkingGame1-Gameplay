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

    public void Animate_CrystalInfoText()
    {

        HandleCrystalInfoPanel(true);

    }
    public void Init()
    {
        HandleCrystalInfoPanel(false);
    }
    public void HandleCrystalInfoPanel(bool activate)
    {
        if(activate)
        {
            CrystalInfoTexts.SetActive(activate);
            CrystalStartInfoBG.SetActive(activate);
        }

    }   
    public void ChangeCrystalInfoRectTransformScale(float scaleValue)
    {

        CrystalInfoRectTransform.localScale=new Vector3(scaleValue,scaleValue,1f);

    }

    private void OnAnimationFinished()
    {
        CloseSmoothly();
        animAction.Invoke();
    }



}
