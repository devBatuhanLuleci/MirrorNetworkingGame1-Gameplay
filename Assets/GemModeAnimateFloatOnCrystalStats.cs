using UnityEngine;

public class GemModeAnimateFloatOnCrystalStats : AnimateVector2
{

    public override void Update()
    {
        base.Update();


    }
    public override void OnCurrentValueUpdated(Vector2 oldValue, Vector2 newValue)
    {
        // Debug.Log($"ezdim: current value : {currentValue}");

        GameplayPanelUIManager.Instance.SetCrystalInfoPanelPos(currentValue);


    }
    public override void OnAnimationFinished()
    {
        base.OnAnimationFinished();


    }
}
