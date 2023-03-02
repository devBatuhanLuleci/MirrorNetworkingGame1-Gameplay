using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatboyHealth : PlayerHealth
{
    float perBarAmount = 0.333f;
  
    public override void RefreshUI(int oldValue, int newValue)
    {
        base.RefreshUI(oldValue, newValue);
        playerController.HealthChanged(newValue);
    }




 

}


