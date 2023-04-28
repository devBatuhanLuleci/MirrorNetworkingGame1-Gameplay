using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalModeCountdown : NetworkedTimer
{

    public override void OnCountdownChangedSync(int oldCountdown, int newCountdown)
    {
        base.OnCountdownChangedSync(oldCountdown, newCountdown);
        GameplayPanelUIManager.Instance.SetCrystalModeCountDownValue(newCountdown);

    }
    public override void OnCountDownFinished()
    {
        base.OnCountDownFinished();

         (NetworkedGameManager.Instance as CrystalModeNetworkedGameManager)?.OnGameFinished();
    }

}