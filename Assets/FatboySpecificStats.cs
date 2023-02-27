using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatboySpecificStats : CharacterSpecificStats
{
    public GameObject BackTurret;

    public override void Handle_Specific_Object_On_Ulti_AttackButtonPressed()
    {
        base.Handle_Specific_Object_On_Ulti_AttackButtonPressed();
        Handle_BackTurret();

    }
    public override void Handle_Specific_Object_On_Basic_AttackButtonPressed()
    {
        base.Handle_Specific_Object_On_Basic_AttackButtonPressed();
    }

    public void Handle_BackTurret()
    {
        BackTurret.SetActive(!BackTurret.activeSelf);
    }
}
