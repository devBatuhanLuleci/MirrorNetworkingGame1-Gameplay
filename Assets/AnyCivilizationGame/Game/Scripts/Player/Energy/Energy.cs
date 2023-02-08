using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Energy : NetworkBehaviour
{
    public int TotalEnergyAmount = 3;

    [SyncVar]
    public int CurrentEnergyAmount;

    public float fillSpeed = .1f;
    private float MaxfillAmount = 1f;

    [SyncVar(hook = nameof(RefreshUI))]
    public float CurrentFillAmount;

    float perBarAmount = 0.333f;


    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        CurrentEnergyAmount = TotalEnergyAmount;
    }


    public void CastEnergy()
    {
       DecreaseEnergy(.333f);


    }
    public bool HaveEnergy()
    {
        bool haveEnergy = true;
        if (CurrentEnergyAmount==0)
        {
            haveEnergy = false;
            playerController.ShakeEnergyBar();

        }
        return haveEnergy;


    }
    public void IncreaseEnergyOverTime()
    {
        if(playerController.attack.isShooting)
        {
            return;
        }

        CurrentFillAmount += Time.deltaTime * fillSpeed;
        if (CurrentFillAmount > MaxfillAmount)
        {

            CurrentFillAmount = MaxfillAmount;
        }


        int value = Mathf.FloorToInt(CurrentFillAmount / perBarAmount);

        if (value != CurrentEnergyAmount)
        {

            CurrentEnergyAmount = value;
        }
        
    }
    public void DecreaseEnergy(float energyAmount)
    {
      
        if (CurrentFillAmount >= energyAmount)
        {
            CurrentFillAmount -= energyAmount;
          
            CurrentEnergyAmount--;

        }
       
    }
   
    public void MakeEnergyBarsFull()
    {
        CurrentFillAmount = 1f;
        CurrentEnergyAmount = TotalEnergyAmount;

    }

    private void Update()
    {
        if (netIdentity.isServer)
        {

            if (CurrentFillAmount <= MaxfillAmount)
            {
                IncreaseEnergyOverTime();
            }


        }


        //if (Input.GetKeyDown(KeyCode.R))
        //{

        //    CastEnergy();
        //}
        //if (Input.GetKeyDown(KeyCode.F))
        //{

        //    MakeEnergyBarsFull();
        //}
    }
    public void RefreshUI(float oldValue,  float newValue)
    {
        playerController.EnergyChanged(newValue);
    }



}
