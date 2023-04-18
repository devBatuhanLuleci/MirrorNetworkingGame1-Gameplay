using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalModeGameTime : NetworkedTimer
{
    public override void OnCountdownChangedSync(int oldCountdown, int newCountdown)
    {
        base.OnCountdownChangedSync(oldCountdown, newCountdown);
        GameplayPanelUIManager.Instance.SetTimeValueOnPanel(newCountdown);


    }
    public override void OnCountDownFinished()
    {
        base.OnCountDownFinished();
       CrystalModeNetworkedGameManager crystalModeNetworkedGameManager =  (CrystalModeNetworkedGameManager)NetworkedGameManager.Instance;
        crystalModeNetworkedGameManager.OnCountdownReached_FinishGame();
    }
}
