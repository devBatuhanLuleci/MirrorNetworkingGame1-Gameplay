using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class TurretSetup : ObjectSetup
{
    Throwable Throwable;
    public override void Start()
    {
        base.Start();

        //if (NetworkIdentity.isServer)
        //{
         
        //    SetColor();

        //}

    }
    //[Command]
    //public void SetColor()
    //{

       
    //        Throwable = GetComponent<Throwable>();
    //        Debug.Log($"root net id:  {Throwable.RootNetId}");
    //        SetTeamColorOfThisObject_RPC(Throwable.RootNetId);
      
    //}
    public override void SetObjectDataForServer()
    {
        base.SetObjectDataForServer();

        var data = ACGDataManager.Instance.GetCharacterData().Attributes;
        try
        {
            if (data.TryGetValue("TurretHealth", out var TurretHealthAttribute))
            {

                health.ResetValues((int)TurretHealthAttribute.Value);
                // objectUIHandler.enabled = false;
            }
            //if (data.TryGetValue("Energy", out var energyAttribute))
            //{
            //    energy.MakeEnergyBarsFull((int)energyAttribute.Value);
            //}
        }
        catch (Exception ex) { Debug.LogError(ex.Message); }

    }
    public override IEnumerator SetTeamColorLocal(uint netId)
    {

        StartCoroutine(base.SetTeamColorLocal(netId));
        yield return null;
    }

}
