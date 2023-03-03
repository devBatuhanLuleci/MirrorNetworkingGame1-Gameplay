using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatboySpecificStats : CharacterSpecificStats
{
    public GameObject BackTurret;

    public override void Handle_Specific_Object_On_Ulti_AttackButtonPressed()
    {
        base.Handle_Specific_Object_On_Ulti_AttackButtonPressed();
        Dectivate_BackTurret();

    }
    public override void Handle_Specific_Object_On_Basic_AttackButtonPressed()
    {
        base.Handle_Specific_Object_On_Basic_AttackButtonPressed();
    }

    public void Activate_BackTurret()
    {
        BackTurret.SetActive(true);
    }
    public void Dectivate_BackTurret()
    {
        BackTurret.SetActive(false);
    }
}
