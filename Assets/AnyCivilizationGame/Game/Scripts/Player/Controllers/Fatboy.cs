using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fatboy : PlayerController
{

    public GameObject UltiTurret;
    public float ZValue ;
    public float YValue;

    public override void Fire(bool isAutoattack, Vector3 dir)
    {
       

    //    ZValue = Mathf.Abs(BulletSpawnPoints[2].spawnPoint.z  -radialOffset);
        ZValue = Mathf.Abs(-radialOffset);
        YValue = Mathf.Abs(BulletSpawnPoints[2].spawnPoint.y);

        base.Fire(isAutoattack, dir);

        switch (currentAttackType)
        {
            case CurrentAttackType.Basic:

                SpawnBullet(new Vector3[] { BulletSpawnPoints[0].spawnPoint, BulletSpawnPoints[1].spawnPoint }, dir.normalized, BulletCount, BulletIntervalTime, ZValue,0);

                break;
            case CurrentAttackType.Ulti:

                SpawnBullet(new Vector3[] { BulletSpawnPoints[2].spawnPoint, }, dir, 1, .1f, ZValue, YValue);


                break;
            default:
                break;
        }

        transform.FindByName("Spine");
    }

    //public override void DetectJoystickButton(SimpleInputNamespace.Joystick.JoystickButtonType joystickButtonType)
    //{


    //    base.DetectJoystickButton(joystickButtonType);



    //}
    public override void Activate_Something_OnBulletObjectSpawned_Before()
    {
        base.Activate_Something_OnBulletObjectSpawned_Before();

        switch (currentAttackType)
        {
            case CurrentAttackType.Basic:

                break;
            case CurrentAttackType.Ulti:

                if (UltiTurret != null)
                {
                    
                    //if (UltiTurret != obj.gameObject)
                    //{
                    if (UltiTurret.TryGetComponent<TurretController>(out TurretController fatboyTurretController))
                    {
                        fatboyTurretController.ActivateTurret();
                        //MatchNetworkManager.Instance.DestroyThis(fatboyTurretController);

                    }

                    // }

                }

                break;
            default:
                break;
        }

   

    }
    public override void OnBulletObjectSpawned(Throwable obj)
    {
        base.OnBulletObjectSpawned(obj);


        if (currentAttackType == CurrentAttackType.Ulti)
        {

          
            DeactivateBackTurret();

            if (UltiTurret != null)
            {
                //if (UltiTurret != obj.gameObject)
                //{
                if (UltiTurret.TryGetComponent<TurretController>(out TurretController fatboyTurretController))
                {
                    //fatboyTurretController.DeactivateTurret();

                    MatchNetworkManager.Instance.DestroyThis(fatboyTurretController);

                }

                // }

            }
            else
            {
                
            }


            UltiTurret = obj.gameObject;
        }
    }
    [ClientRpc]
    public void DeactivateBackTurret()
    {
      

        var stats = CharacterSpecificStats as FatboySpecificStats;
        stats.Dectivate_BackTurret();
    }

    public override void OnThisObjectDestroyed()
    {
        base.OnThisObjectDestroyed();
        UltiTurret = null;
    }

}
