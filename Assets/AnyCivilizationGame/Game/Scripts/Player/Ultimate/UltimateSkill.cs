using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSkill : NetworkBehaviour
{


    public float TotalFillAmount = 100;

    [SyncVar(hook = nameof(RefreshUI))]
    public float CurrentFillAmount;

    private PlayerController playerController;


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        if (netIdentity.isServer)
        {

            CurrentFillAmount = TotalFillAmount;
        }
    }

    public void IncreaseCurrentUltimateFillAmount(NetworkConnection target, float UltimateFillAmountRate)
    {

        if (CurrentFillAmount == TotalFillAmount) return;


        CurrentFillAmount += UltimateFillAmountRate;

        if (CurrentFillAmount >= TotalFillAmount)
        {
            CurrentFillAmount = TotalFillAmount;
            playerController.ActivateUlti(target);
        }

    }
    
    public void ResetCurrentFillAmount()
    {

   
        CurrentFillAmount = 0;

    }
    public void RefreshUI(float oldValue, float newValue)
    {
        playerController.OnCurrentUltimateFillRateChanged(newValue);
    }


}
