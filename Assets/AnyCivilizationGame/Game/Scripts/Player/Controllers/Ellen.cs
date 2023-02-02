using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ellen : PlayerController
{
    public override void Fire(bool isAutoattack, Vector3 dir)
    {
        base.Fire(isAutoattack, dir);


        switch (currentAttackType)
        {
            case CurrentAttackType.Basic:

                SpawnBullet(new Vector3[] { BulletSpawnPoints[0].BulletInitPos, BulletSpawnPoints[1].BulletInitPos }, dir, BulletCount, BulletIntervalTime);

                break;
            case CurrentAttackType.Ulti:

                SpawnBullet(new Vector3[] { BulletSpawnPoints[2].BulletInitPos, }, dir, 1, .1f);


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

}
