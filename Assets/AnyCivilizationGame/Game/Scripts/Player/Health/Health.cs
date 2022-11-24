using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public int MaxHealth { get; private set; } = 100;

    [SyncVar]
    public int Value;

    public void TakeDamage(int damage)
    {
        var newHealth = Value - damage;
        Value = Mathf.Clamp(newHealth, 0, 100);
        if (Value <= 0)
        {
            Debug.LogError("Dath");
        }
    }

    public void ResetValues()
    {
        Debug.LogError("maxHelath: " + MaxHealth);
        Value = MaxHealth;
    }

    public void RefreshUI()
    {
        // TODO: Health bar? güncelle.
    }

}
