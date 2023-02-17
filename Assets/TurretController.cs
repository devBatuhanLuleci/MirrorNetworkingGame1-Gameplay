using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : ObjectController
{
    public ObjectUIHandler turretUIHandler;
    private FatboyTurret fatboyTurret;

    public override void Awake()
    {
        base.Awake();
        fatboyTurret = GetComponent<FatboyTurret>();
    }
    public override void HealthChanged(int health)
    {
        base.HealthChanged(health);
        turretUIHandler.ChangeHealth(health);

    }
    public override void Death()
    {
        base.Death();
        MatchNetworkManager.Instance.DestroyThis(this);

    }


    [ClientRpc]
    public override void DeathRPC()
    {

        base.DeathRPC();

        // TODO: show death panel if localplayer
        //if (netIdentity.isLocalPlayer)
        //{
        //    infoPopup = InfoPopup.Show("Loser! You will respawn in 3 second.");

        //}
    }
    public override void DestroyThisObject()
    {
      
        foreach (var item in FindObjectsOfType<NetworkIdentity>(true))
        {
            if(item.netId== fatboyTurret.OwnerNetId)
            {
                if(item.transform.TryGetComponent(out ObjectController objectController))
                {
                objectController.OnThisObjectDestroyed();

                }


            }
        }
        fatboyTurret.turretShootStatus = FatboyTurret.TurretShootStatus.Idle;
        fatboyTurret.turretRotation = FatboyTurret.TurretRotation.Idle;

        fatboyTurret.waitForIt = fatboyTurret.InitwaitForIt;
        fatboyTurret.CurrentTarget = null;

        base.DestroyThisObject();

    }
}
