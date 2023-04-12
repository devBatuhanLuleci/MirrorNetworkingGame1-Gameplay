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

    private RectTransform CrystalInfoRectTransform;
    public UnityAction animAction;


    public void Animate_CrystalInfoText()
    {

        CrystalInfoTexts.gameObject.SetActive(true);
        CrystalInfoRectTransform= CrystalInfoTexts.GetComponent<RectTransform>();

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
