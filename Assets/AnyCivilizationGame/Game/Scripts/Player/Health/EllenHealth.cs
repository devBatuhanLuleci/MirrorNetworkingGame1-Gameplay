using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllenHealth : Health
{
    private PlayerController playerController;

    public override void Awake()
    {
        base.Awake();
        playerController = GetComponent<PlayerController>();
    }
    public override void RefreshUI(int oldValue, int newValue)
    {
        base.RefreshUI(oldValue, newValue);
        playerController.HealthChanged(newValue);
    }
}
