using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Health : NetworkBehaviour
{
    public int MaxHealth { get; private set; } = 50;

    [SyncVar(hook = nameof(RefreshCurrentHealth))]
    public int currentHealht ;

    [SyncVar(hook = nameof(RefreshHealthRate))]
    public float HealthRate;


    private PlayerController playerController;

    public virtual void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public virtual void Update()
    {
        //Inherited


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
    public virtual void RefreshCurrentHealth(int oldValue, int newValue)
    {
      
    }
    public virtual void RefreshHealthRate(float oldValue, float newValue)
    {
       
    }
    #endregion
}
