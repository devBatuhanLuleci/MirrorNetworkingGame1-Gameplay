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
        // m_MyFirstAction.AddListener(m_MyFirstAction);
        // CrystalInfoRectTransform.DOScale(1f, 1f).From(0f).SetLoops(2,LoopType.Yoyo).SetEase(Ease.OutQuad).SetDelay(2);

      StartCoroutine(HandleScaleOfCrystalInfoRectTransformCoroutine());
    }
    public IEnumerator HandleScaleOfCrystalInfoRectTransformCoroutine()
    {
        float elapsedTime = 0f;
        float duration = 1.5f;

        // Scale up
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float scale = Mathf.Lerp(0f, 1f, t);
            CrystalInfoRectTransform.localScale = new Vector3(scale, scale, 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        CrystalInfoRectTransform.localScale = Vector3.one;

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Scale down
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float scale = Mathf.Lerp(1f, 0f, t);
            CrystalInfoRectTransform.localScale = new Vector3(scale, scale, 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        CrystalInfoRectTransform.localScale = Vector3.zero;


        OnAnimationFinished();
    }
    //public void SequenceRectTransformScale()
    //{
    //    Sequence s = DOTween.Sequence();
    //    s.Append(CrystalInfoRectTransform.DOScale(1f, 1.5f).From(0f).SetEase(Ease.OutBack));
    //    s.AppendInterval(1f);
    //    s.Append(CrystalInfoRectTransform.DOScale(0f, 1.5f).SetEase(Ease.InBack));
    //    s.SetDelay(2f);
     
    //    s.OnComplete(() => {
    //        // Debug.Log("Animasyon tamamlandı.");
    //        OnAnimationFinished();
    //    });
    //}

    private void OnAnimationFinished()
    {
        CloseSmoothly();
        animAction.Invoke();
    }



}
