using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public int MaxHealth { get; private set; } = 100;
    [SyncVar(hook = nameof(RefreshCurrentHealth))]
    public int currentHealht = 100;

    [SyncVar(hook = nameof(RefreshHealthRate))]
    public float HealthRate;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public bool TakeDamage(int damage)
    {
        var newHealth = currentHealht - damage;
        currentHealht = Mathf.Clamp(newHealth, 0, MaxHealth);
        HealthRate = currentHealht / (float)MaxHealth;
        if (currentHealht <= 0) return true;
        return false;
    }
    public void ResetValues()
    {
        currentHealht = MaxHealth;
        HealthRate = 1;
    }
    public void ResetValues(int value)
    {
        currentHealht = MaxHealth = value;
        HealthRate = 1;
    }

    #region Hooks
    public void RefreshCurrentHealth(int oldValue, int newValue)
    {
        playerController.HealthChanged(newValue);
    }
    public void RefreshHealthRate(float oldValue, float newValue)
    {
        playerController.HealthRateChanged(newValue);
    }
    #endregion
}
