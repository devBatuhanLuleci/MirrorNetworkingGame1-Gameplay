using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ellen : PlayerController
{
    public override void Fire(bool isAutoattack, Vector3 dir)
    {
        base.Fire(isAutoattack, dir);

        SpawnBullet(new Vector3[]{ BulletSpawnPoints[0].BulletInitPos, BulletSpawnPoints[1].BulletInitPos }, dir,BulletCount,BulletIntervalTime);
       
           
    }
}
