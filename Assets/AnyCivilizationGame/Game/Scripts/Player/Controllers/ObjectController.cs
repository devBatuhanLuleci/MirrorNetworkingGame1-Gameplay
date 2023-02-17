using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectController : NetworkBehaviour, INetworkPooledObject
{

    [HideInInspector]
    public Health health;

    [SerializeField]
    public bool IsLive { get; protected set; } = true;

    public Action ReturnHandler { get; set; }

    public virtual void Awake()
    {
        health = GetComponent<Health>();
    }


    public virtual void TakeDamage(int damage)
    {
        if (health.TakeDamage(damage))
        {
            // TODO: Player is death 
            // respawn player
            Death();
        }
    }
    public virtual void HealthChanged(int health)
    {

    }

    /// <summary>
    /// this method must call from server
    /// </summary>
    public virtual void Death()
    {
        IsLive = false;
        // MatchNetworkManager.Instance.Respawn(this);
        DeathRPC();


    }


    [ClientRpc]
    public virtual void DeathRPC()
    {


        IsLive = false;
        // TODO: show death panel if localplayer


    }



    public virtual void DestroyThisObject()
    {

        health.ResetValues();
        IsLive = true;
        DestroyThisObjectRPC();

        NetworkServer.UnSpawn(gameObject);

        ReturnHandler();


        //  gameObject.SetActive(false);

    }

    public virtual void OnThisObjectDestroyed()
    {

    }

    [ClientRpc]
    public void DestroyThisObjectRPC()
    {

        IsLive = true;
        //  gameObject.SetActive(false);
        // NetworkServer.UnSpawn(gameObject);
        // ReturnHandler();

    }
}
