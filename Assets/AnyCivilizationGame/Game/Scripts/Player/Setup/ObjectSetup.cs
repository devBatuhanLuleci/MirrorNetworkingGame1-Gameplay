using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
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

        if (!NetworkIdentity.isServer)
        { 
        
         objectUIHandler.Initialize(health.MaxHealth);
            health.ResetValues(100);
        }
        else
        {
            health.ResetValues(100);
            objectUIHandler.enabled = false;


        }
    }
}