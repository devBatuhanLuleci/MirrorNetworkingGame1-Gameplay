using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;
using System;

public class CrystalModeGamePlayCanvasUIController : NetworkedPanel
{
    public CrystalStatsUIPanelManager GemUIPanelManager;
  
    private RectTransform GemRectTransform;

  
   

  


    public CrystalStartInfoPanel crystalStartInfoPanel;
    private void Awake()
    {

       GemRectTransform = GemUIPanelManager.GetComponent<RectTransform>();
        // OnStart_MoveDown_TeamUIPanel();

        if(crystalStartInfoPanel!=null)
        {
            crystalStartInfoPanel.animAction += OnAnimationFinished;
        }
    }

    
    public void Activate () {
    
    
        crystalStartInfoPanel.Animate_CrystalInfoText();


    }
    public override void Show()
    {
        // base.Show();
        //Activate();
        gameObject.SetActive(true);
        Activate();
        // StartCoroutine(Test());

    }
    private void OnDestroy()
    {
        if (crystalStartInfoPanel != null)
        {
            crystalStartInfoPanel.animAction -= OnAnimationFinished;
        }
    }
    public void OnAnimationFinished()
    {
        Debug.Log("animation bitti!");
        
        OnStart_MoveDown_TeamUIPanel();
    }
    //IEnumerator Test()
    //{
    //    ChangeModeInfo(CanvasSequence.ModeInfo);
    //    Activate();
    //      // GemRectTransform.gameObject.SetActive(true);
    //      yield return new WaitForSeconds(5);

    //    ChangeModeInfo(CanvasSequence.InGame);

    //    //  GemRectTransform.gameObject.SetActive(false);

    //}


    public void OnStart_MoveDown_TeamUIPanel()
    {
        GemRectTransform.gameObject.SetActive(true);
        GemRectTransform?.DOAnchorPos(new Vector3(0, -50, 0), 1f).SetEase(Ease.OutQuad);




    }
    
}
