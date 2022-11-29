using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class PlayerController : NetworkBehaviour
{

    #region     Private Fields
    private PlayerMovement movement;
    private PlayerAttack attack;
    private Health health;
    [SerializeField]
    public bool IsLive { get; private set; } = true;

    private InfoPopup infoPopup;
    #endregion

    [HideInInspector]
    public PlayerUIHandler playerUIHandler;

    #region Character Projectile Details
    public Transform FirePoint;
    public Transform TargetPoint;
    public LineRenderer temporaryLine;
    public float initialVelocity = 1f;


    #endregion



    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<Health>();
        playerUIHandler = GetComponent<PlayerUIHandler>();
    }

    public virtual void SetParametersForShoot()
    {

    }
    #region Input Methods

    /// <summary>
    /// this method must call from server
    /// </summary>
    public virtual void TakeDamage(int damage)
    {
        if (health.TakeDamage(damage))
        {
            // TODO: Player is death 
            // respawn player
            Death();
        }
    }
    /// <summary>
    /// this method must call from server
    /// </summary>
    private void Death()
    {
        IsLive = false;
        MatchNetworkManager.Instance.Respawn(this);
        DeathRPC();
    }

    /// <summary>
    /// this method must call from server
    /// </summary>
    public void Respawn()
    {
        IsLive = true;
        transform.position = Vector3.zero;
        health.ResetValues();
        RespawnRPC();
    }

    public void HealthChanged(int health)
    {
        playerUIHandler.ChangeHealth(health);
    }
    public virtual void Move(Vector2 move)
    {
        movement.MovementSpriteHandler(move);
        if (move.sqrMagnitude > 0f)
        {
            MoveCmd(move);
        }
        //Debug.Log($"<b> sqrM: {move.sqrMagnitude} </b> move:" + move);
    }
    public void Targeting(Vector2 targetingDirection, bool held = false)
    {
        attack.Targeting(targetingDirection, held);
    }
    #endregion

    #region RPC Methods
    [Command]
    public virtual void MoveCmd(Vector2 move)
    {
        movement.Move(move);
    }


    [ClientRpc]
    public void DeathRPC()
    {
        IsLive = false;
        // TODO: show death panel if localplayer
        if (netIdentity.isLocalPlayer)
        {
            infoPopup = InfoPopup.Show("Loser! You will respawn in 3 second.");
        }
    }
    [ClientRpc]
    public void RespawnRPC()
    {
        IsLive = true;
        // TODO: hide death panel

        if (netIdentity.isLocalPlayer && infoPopup != null)
        {
            infoPopup.Close();
        }
    }


    #endregion
}
