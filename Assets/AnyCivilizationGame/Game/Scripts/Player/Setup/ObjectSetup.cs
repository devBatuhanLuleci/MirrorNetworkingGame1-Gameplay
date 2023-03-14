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
}
