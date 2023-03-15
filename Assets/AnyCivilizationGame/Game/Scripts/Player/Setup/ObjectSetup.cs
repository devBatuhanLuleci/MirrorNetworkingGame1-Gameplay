using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using static UnityEngine.EventSystems.EventTrigger;

public class ObjectSetup : NetworkBehaviour
{
    protected NetworkIdentity NetworkIdentity;
    protected ObjectUIHandler objectUIHandler;
    protected Health health;

    public virtual void Awake()
    {
        NetworkIdentity = GetComponent<NetworkIdentity>();
        health = GetComponent<Health>();
        objectUIHandler = GetComponent<ObjectUIHandler>();
    }

    public virtual void Start()
    {
        // Do anything on all client but not server
        if (!NetworkIdentity.isServer)
        {
            objectUIHandler.Initialize();
        }
        else // Do anything on server
        {
            objectUIHandler.enabled = false;
      
        }
    }
    public virtual void SetObjectDataForServer()
    {
     
    }
    public virtual IEnumerator SetTeamColorLocal( uint netId)
    {
    
        if (NetworkedGameManager.Instance.IsInMyTeam(netId))
        {
           // playerController.playerUIHandler.Change_TeamIndicator_Color("Ally");
            objectUIHandler.Change_TeamHealthBar_Color("Ally");

        }
        else
        {
            //  playerController.playerUIHandler.Change_TeamIndicator_Color("Enemy");
            objectUIHandler.Change_TeamHealthBar_Color("Enemy");
        }
       // yield return new WaitForSeconds(3);
        yield return null;
    }
    [ClientRpc]
    public void SetTeamColorOfThisObject_RPC(uint netId)
    {
      
        StartCoroutine(SetTeamColorLocal(netId));


    }
    [ClientRpc]
    public void SetIndicatorColorOfThisObject_RPC(uint netId)
    {

        if (NetworkedGameManager.Instance.IsInMyTeam(netId))
        {
            objectUIHandler.Change_TeamIndicator_Color("Ally");
          //  objectUIHandler.Change_TeamHealthBar_Color("Ally");

        }
        else
        {
            objectUIHandler.Change_TeamIndicator_Color("Enemy");
          //  objectUIHandler.Change_TeamHealthBar_Color("Enemy");

        }


    }
}
