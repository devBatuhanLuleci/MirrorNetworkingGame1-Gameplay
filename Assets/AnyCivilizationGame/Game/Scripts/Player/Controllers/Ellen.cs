using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ellen : PlayerController
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
    public override void OnBulletObjectSpawned(Throwable obj)
    {
        base.OnBulletObjectSpawned(obj);


        if (currentAttackType == CurrentAttackType.Ulti)
        {


            if (UltiTurret != null)
            {
                //if (UltiTurret != obj.gameObject)
                //{
                if (UltiTurret.TryGetComponent<TurretController>(out TurretController fatboyTurretController))
                {
                    MatchNetworkManager.Instance.DestroyThis(fatboyTurretController);

                }

                // }

            }

            UltiTurret = obj.gameObject;
        }
    }

    public override void OnThisObjectDestroyed()
    {
        base.OnThisObjectDestroyed();
        UltiTurret = null;
    }

}
