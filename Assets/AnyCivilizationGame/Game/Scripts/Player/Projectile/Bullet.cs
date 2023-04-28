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
      

        if (other.TryGetComponent<PlayerController>(out var otherPlayerController) && CanAttackToThisPlayer(otherPlayerController))
        {
            if (NetworkedGameManager.Instance.isGameFinished) { return; }
            if (otherPlayerController.IsLive)
            {

                PlayerController OurPlayer = MatchNetworkManager.Instance.GetPlayerByConnectionID(OwnerConnectionId);

                otherPlayerController.ChangeDamageTakenStatus(PlayerController.DamageTakenStatus.IsTakenDamage);
                otherPlayerController.TakeDamage(damage, otherPlayerController.netIdentity.connectionToClient);

                OurPlayer.ultimateSkill.IncreaseCurrentUltimateFillAmount(OurPlayer.netIdentity.connectionToClient, damage * 4);
             
                NetworkServer.UnSpawn(gameObject);
                ReturnHandler();

            }

        }

        if (other.TryGetComponent<IDamagable>(out IDamagable damagableObject)/* && CanAttack(damagableObject.)*/)
        {

            if (NetworkedGameManager.Instance.isGameFinished) { return; }
            if (other.TryGetComponent<FatboyTurret>(out FatboyTurret fatboyTurret))
            {

                if (isEnemy(fatboyTurret))
                {

                    PlayerController OurPlayer = MatchNetworkManager.Instance.GetPlayerByConnectionID(OwnerConnectionId);
                    OurPlayer.ultimateSkill.IncreaseCurrentUltimateFillAmount(OurPlayer.netIdentity.connectionToClient, damage * 4);

                    damagableObject.GetDamage(10);

                    NetworkServer.UnSpawn(gameObject);
                    ReturnHandler();

                }

            }



        }

    }

    private bool isEnemy(FatboyTurret fatboyTurret)
    {
        var isEnemy = false;

        isEnemy = OwnerNetId != 0 && RootNetId != fatboyTurret.RootNetId/* bullet kendi turret'ına mı değdi  */&& !NetworkedGameManager.Instance.IsInMyTeam(RootNetId, fatboyTurret.RootNetId) && netIdentity.isServer;




        return isEnemy;
    }
    private bool CanAttackToThisPlayer(PlayerController player)
    {
        // return OwnerNetId != 0 && playerController.netIdentity.netId != OwnerNetId && RootNetId != playerController.netIdentity.netId && netIdentity.isServer;
        return isNotMe(player) && !NetworkedGameManager.Instance.IsInMyTeam(RootNetId, player.netIdentity.netId) && netIdentity.isServer;
    }
    private bool isNotMe(PlayerController player)
    {
        var isNotMe = false;

        isNotMe = OwnerNetId != 0 && RootNetId != player.netId && netIdentity.isServer;




        return isNotMe;
    }


    public override void OnArrived()
    {
        NetworkServer.UnSpawn(gameObject);
        ReturnHandler();

    }

}
