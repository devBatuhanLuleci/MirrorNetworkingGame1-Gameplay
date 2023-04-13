using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;
using System;
using static UnityEngine.UI.CanvasScaler;

public class CrystalModeGamePlayCanvasUIController : Panel
{
  
    private RectTransform GemRectTransform;

  
   

  

    public CrystalStatsUIPanelManager crystalStatsUIPanel;

    public CrystalStartInfoPanel crystalStartInfoPanel;
    private void Awake()
    {

       GemRectTransform = crystalStatsUIPanel.GetComponent<RectTransform>();
     
        // OnStart_MoveDown_TeamUIPanel();

        if(crystalStartInfoPanel!=null)
        {
            crystalStartInfoPanel.animAction += OnAnimationFinished;
        }
    }

    
    public void Activate (bool isActive) {
    
    
        crystalStartInfoPanel.Animate_CrystalInfoText(isActive);
      //  OnStart_MoveDown_TeamUIPanel();

    }
    public void Init() {

        crystalStartInfoPanel.Init();



    }
    public void HandleCrystalInfoText(float value)
    {

        crystalStartInfoPanel.ChangeCrystalInfoRectTransformScale(value);
    }
    public void HandleCrystalInfoPanelPos(Vector2 value)
    {
        crystalStatsUIPanel.ChangeCrystalStatsRectTransformPos(value);
     //   crystalStartInfoPanel.ChangeCrystalInfoRectTransformScale(value);
    }


    public override void Show()
    {

     //   Activate();
    

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

    }
   

    public void OnStart_MoveDown_TeamUIPanel()
    {
        GemRectTransform.gameObject.SetActive(true);




    }
 

}
