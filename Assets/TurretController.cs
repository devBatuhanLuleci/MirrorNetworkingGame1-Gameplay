using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : ObjectController
{
    public ObjectUIHandler turretUIHandler;
    [HideInInspector]
    public FatboyTurret fatboyTurret;

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
    public void HealthRateChanged(float newValue)
    {
        turretUIHandler.ChangeHealthRate(newValue);
    }
 
    public override void Death()
    {
        ActivateTurret();
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
            if (item.netId == fatboyTurret.OwnerNetId)
            {
                if (item.transform.TryGetComponent(out ObjectController objectController))
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


    public void ActivateTurret()
    {
     //   PlayerController player = MatchNetworkManager.Instance.GetPlayerByNetID(fatboyTurret.RootNetId);
        PlayerController player = MatchNetworkManager.Instance.GetPlayerByConnectionID(fatboyTurret.OwnerConnectionId);
        Activate(player);

    }
    public void DeactivateTurret()
    {
     //   PlayerController player = MatchNetworkManager.Instance.GetPlayerByNetID(fatboyTurret.RootNetId);
        PlayerController player = MatchNetworkManager.Instance.GetPlayerByConnectionID(fatboyTurret.OwnerConnectionId);
        Deactivate(player);

    }
    //[ClientRpc(includeOwner = true)]
    //public override void DestroyThisObjectRPC()
    //{

    //    base.DestroyThisObjectRPC();
    //    ActivateTurretOnDestroy();
    //}
    [ClientRpc]
    public void Activate(PlayerController player)
    {

        var stats = player.CharacterSpecificStats as FatboySpecificStats;
        //Debug.Log(stats.name);

        stats.Activate_BackTurret();
    }
    [ClientRpc]
    public void Deactivate(PlayerController player)
    {

        var stats = player.CharacterSpecificStats as FatboySpecificStats;
        //Debug.Log(stats.name);

        stats.Dectivate_BackTurret();
    }
}
