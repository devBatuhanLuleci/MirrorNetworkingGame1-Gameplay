using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;
using System;

public class Bullet : Throwable, INetworkPooledObject
{
    public int damage = 5;





    public Action ReturnHandler { get; set; }



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
          //  gameObject.SetActive(false);
            Debug.Log("some one hited by " + OwnerName);
            NetworkServer.UnSpawn(gameObject);
            ReturnHandler();

        }
     
            if (other.TryGetComponent<IDamagable>(out IDamagable damagableObject)/* && CanAttack(damagableObject.)*/)
        {
            //TODO: buraya girmiyor. bak
          //  Debug.Log("hello hawagi ");
            if (other.TryGetComponent<FatboyTurret>(out FatboyTurret fatboyTurret))
            {

                if (isEnemy(fatboyTurret))
                {
                    //TODO: burada server'da nul referance hatası alıyoruz düzelt.
                    damagableObject.GetDamage(10);
                    // otherPlayerController.TakeDamage(damage);
                  //  gameObject.SetActive(false);
                    // Debug.Log("some one hited by " + OwnerName);
                    NetworkServer.UnSpawn(gameObject);
                    ReturnHandler();

                }

            }

         

        }

    }
    private bool isEnemy(FatboyTurret fatboyTurret)
    {
        var isEnemy = false;

        isEnemy = OwnerNetId != 0 && RootNetId != fatboyTurret.RootNetId/* bullet kendi turret'ına mı değdi  */&& fatboyTurret.netIdentity.netId != OwnerNetId && netIdentity.isServer;




        return isEnemy;
    }
    private bool CanAttack(PlayerController playerController)
    {
        return OwnerNetId != 0 && playerController.netIdentity.netId != OwnerNetId && RootNetId != playerController.netIdentity.netId && netIdentity.isServer;
    }



    public override void OnArrived()
    {
        NetworkServer.UnSpawn(gameObject);
        ReturnHandler();

    }

}
