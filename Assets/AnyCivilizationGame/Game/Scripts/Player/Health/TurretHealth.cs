using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : Health
{
    private TurretController turretController;

    public override void Awake()
    {
        base.Awake();
        turretController = GetComponent<TurretController>();
    }
    public override void RefreshUI(int oldValue, int newValue)
    {
        base.RefreshUI(oldValue, newValue);
        turretController.HealthChanged(newValue);
    }

}
