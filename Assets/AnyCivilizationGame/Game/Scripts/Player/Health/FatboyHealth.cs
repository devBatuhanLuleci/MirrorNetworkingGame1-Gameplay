using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatboyHealth : PlayerHealth
{
    float perBarAmount = 0.333f;
  
    public override void RefreshCurrentHealth(int oldValue, int newValue)
    {
        base.RefreshCurrentHealth(oldValue, newValue);
        playerController.HealthChanged(newValue);
    }




 

}


