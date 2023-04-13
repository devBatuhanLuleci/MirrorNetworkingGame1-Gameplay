using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;
using System;
using static UnityEngine.UI.CanvasScaler;

public class CrystalModeGamePlayCanvasUIController : Panel
{
    public CrystalStatsUIPanelManager GemUIPanelManager;
  
    private RectTransform GemRectTransform;

  
   

  


    public CrystalStartInfoPanel crystalStartInfoPanel;
    private void Awake()
    {

       GemRectTransform = GemUIPanelManager.GetComponent<RectTransform>();
        Init();
        // OnStart_MoveDown_TeamUIPanel();

        if(crystalStartInfoPanel!=null)
        {
            crystalStartInfoPanel.animAction += OnAnimationFinished;
        }
    }

    
    public void Activate () {
    
    
        crystalStartInfoPanel.Animate_CrystalInfoText();


    }
    public void Init() {

        crystalStartInfoPanel.Init();



    }
    public void HandleCrystalInfoText(float value)
    {

        crystalStartInfoPanel.ChangeCrystalInfoRectTransformScale(value);
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
   

    public void OnStart_MoveDown_TeamUIPanel()
    {
        GemRectTransform.gameObject.SetActive(true);
        GemRectTransform?.DOAnchorPos(new Vector3(0, -50, 0), 1f).SetEase(Ease.OutQuad);




    }
    
}
