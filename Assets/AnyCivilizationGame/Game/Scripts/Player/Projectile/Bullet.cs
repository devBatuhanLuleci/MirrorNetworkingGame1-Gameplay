using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;
using System;

public class Bullet : Throwable, INetworkPooledObject
{
    public int damage = 50;



    [SerializeField]
    private float speed = 0.5f;

    public Action ReturnHandler { get; set; }

    private void Awake()
    {
        base.speed = speed;
    }

    /// <summary>
    /// This method calls from when the bullet spawn from pool.
    /// </summary>
    /// <param name="rotAngle"></param>
    public override void OnObjectSpawn()
    {
        //  Debug.Log("rotangle: " + rotAngle);
        base.OnObjectSpawn();
      
    }
    /// <summary>
    /// This function sets the rotation of bullet when it is spawn.
    /// </summary>
    /// <param name="rotAngle"></param>
 

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var otherPlayerController) && CanAttack(otherPlayerController))
        {
            otherPlayerController.TakeDamage(damage);
            gameObject.SetActive(false);
            Debug.Log("some one hited by " + OwnerName);
            NetworkServer.UnSpawn(gameObject);
            ReturnHandler();

        }
    }

    private bool CanAttack(PlayerController playerController)
    {
        return OwnerNetId != 0 && playerController.netIdentity.netId != OwnerNetId && netIdentity.isServer;
    }

   

    public override void OnArrived()
    {
        NetworkServer.UnSpawn(gameObject);
        ReturnHandler();
      
    }

}
