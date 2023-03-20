using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSkill : NetworkBehaviour
{


    public float TotalFillAmount = 100;

    [SyncVar(/*hook = nameof(RefreshUI)*/)]
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

        if (CurrentFillAmount < TotalFillAmount)
        {
            bool isBiggerThenMaxAmount = CurrentFillAmount + UltimateFillAmountRate > TotalFillAmount;
            CurrentFillAmount = isBiggerThenMaxAmount ? TotalFillAmount : (CurrentFillAmount + UltimateFillAmountRate);

          
            playerController.HandleUltiFillAmount(target, CurrentFillAmount);
        
            if (CurrentFillAmount  == TotalFillAmount)
            {

                playerController.ActivateUlti(target);
             
            }
            
        }





    }
    public bool IsUltiCanShootable()
    {
        return playerController.isUltiThrowable;
    }

    public void ResetCurrentFillAmount(NetworkConnection target)
    {

        CurrentFillAmount = 0;
        playerController.HandleUltiFillAmount(target, CurrentFillAmount);


    }
    //public void RefreshUI(float oldValue, float newValue)
    //{
    //    playerController.OnCurrentUltimateFillRateChanged(newValue);
    //}


}
