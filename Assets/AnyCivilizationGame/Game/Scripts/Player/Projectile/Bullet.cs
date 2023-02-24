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
            if (otherPlayerController.IsLive)
            {



                otherPlayerController.TakeDamage(damage);

                PlayerController OurPlayer = MatchNetworkManager.Instance.GetPlayerByNetID(RootNetId);

                // if (OurPlayer != null)
                //    ActivateUltiOnTargetObject(OurPlayer.netIdentity.connectionToClient, OurPlayer);
                OurPlayer.ultimateSkill.IncreaseCurrentUltimateFillAmount(OurPlayer.netIdentity.connectionToClient, damage * 4);
                //  gameObject.SetActive(false);
                Debug.Log("some one hited by " + OwnerName);
                NetworkServer.UnSpawn(gameObject);
                ReturnHandler();

            }
            //foreach (var player in MatchNetworkManager.Instance.players)
            //{
            //    Debug.Log($"  1: { player.Key}       2: { player.Value}");
            //}
        }

        if (other.TryGetComponent<IDamagable>(out IDamagable damagableObject)/* && CanAttack(damagableObject.)*/)
        {

            //TODO: buraya girmiyor. bak
            //  Debug.Log("hello hawagi ");
            if (other.TryGetComponent<FatboyTurret>(out FatboyTurret fatboyTurret))
            {

                if (isEnemy(fatboyTurret))
                {

                    PlayerController OurPlayer = MatchNetworkManager.Instance.GetPlayerByNetID(RootNetId);
                    OurPlayer.ultimateSkill.IncreaseCurrentUltimateFillAmount(OurPlayer.netIdentity.connectionToClient, damage * 4);

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
    //[TargetRpc]
    //public void ActivateUltiOnTargetObject(NetworkConnection target, PlayerController player)
    //{
    //   //TODO : Burada ultiyi aktive ediyoruz değiştir. burada kullanıcının attack barını doldur.
    //    player.ActivateUlti();
    //    // This will appear on the opponent's client, not the attacking player's
    //    Debug.Log("Magic Damage  sb =");
    //}

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
