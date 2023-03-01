using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public int MaxHealth { get; private set; } = 100;

    [SyncVar(hook = nameof(RefreshUI))]
    public int Value;

   
   public virtual void Awake()
    {

    }

    public virtual void Update()
    {
        //Inherited


    }
    public bool TakeDamage(int damage)
    {
        var newHealth = Value - damage;
        Value = Mathf.Clamp(newHealth, 0, 100);
        if (Value <= 0)
        {
            Debug.LogError("Dath");
            return true;
        }
        return false;
    }
    public void ResetValues()
    {
        Value = MaxHealth;
    }
    public void ResetValues(int value)
    {
        Value = MaxHealth = value;
    }

    public virtual void RefreshUI(int oldValue, int newValue)
    {
      
    }

}
