using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemModeAnimateFloatOnCountDownTeamInfoPanel : AnimateFloat
{


    public override void Update()
    {
        base.Update();


    
    }
    public override void OnCurrentValueUpdated(float oldValue, float newValue)
    {
        // Debug.Log($"ezdim: current value : {currentValue}");

        GameplayPanelUIManager.Instance.SetCrystalModeCountDownTeamInfoPanelScaleValue(newValue);


    }
    public override void OnAnimationFinished()
    {

        base.OnAnimationFinished();
   

    }

}
