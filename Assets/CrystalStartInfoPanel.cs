using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

public class CrystalStartInfoPanel : Panel
{
    public GameObject CrystalInfoTexts;
    private RectTransform CrystalInfoRectTransform;
    public UnityAction animAction;


    public void Animate_CrystalInfoText()
    {

        CrystalInfoTexts.gameObject.SetActive(true);
        CrystalInfoRectTransform= CrystalInfoTexts.GetComponent<RectTransform>();
        // m_MyFirstAction.AddListener(m_MyFirstAction);
        // CrystalInfoRectTransform.DOScale(1f, 1f).From(0f).SetLoops(2,LoopType.Yoyo).SetEase(Ease.OutQuad).SetDelay(2);

        Sequence s = DOTween.Sequence();
        s.Append(CrystalInfoRectTransform.DOScale(1f, 1.5f).From(0f).SetEase(Ease.OutBack));
        s.AppendInterval(1f);
        s.Append(CrystalInfoRectTransform.DOScale(0f, 1.5f).SetEase(Ease.InBack));
        s.SetDelay(2f);

        s.OnComplete(() => {
           // Debug.Log("Animasyon tamamlandı.");
            OnAnimationFinished();
        });

    }
    private void OnAnimationFinished()
    {
        CloseSmoothly();
        animAction.Invoke();
    }



}
