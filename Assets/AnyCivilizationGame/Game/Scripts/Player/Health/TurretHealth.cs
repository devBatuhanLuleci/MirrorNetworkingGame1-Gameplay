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
    public override void RefreshCurrentHealth(int oldValue, int newValue)
    {
      
        base.RefreshCurrentHealth(oldValue, newValue);
        turretController.HealthChanged(newValue);

    }
    public override void RefreshHealthRate(float oldValue, float newValue)
    {
        base.RefreshHealthRate(oldValue, newValue);
        turretController.HealthRateChanged(newValue);
    }
}
