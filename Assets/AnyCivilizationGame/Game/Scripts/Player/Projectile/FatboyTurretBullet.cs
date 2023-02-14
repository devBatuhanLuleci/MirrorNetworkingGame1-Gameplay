using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatboyTurretBullet : Bullet
{
    [SerializeField]
    private float Speed = .2f;
    private void Awake()
    {
        base.speed = Speed;
    }


    public override void OnArrived()
    {
       base.OnArrived();
    }
}
