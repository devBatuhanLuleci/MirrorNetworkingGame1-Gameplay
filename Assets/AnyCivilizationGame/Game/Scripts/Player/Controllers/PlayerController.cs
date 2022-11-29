using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public  class PlayerController : NetworkBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;
    private Health health;
    [HideInInspector]
    public PlayerUIHandler playerUIHandler;
    [HideInInspector]
    public Animator PlayerAnimatorController;



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
        PlayerAnimatorController = GetComponent<Animator>();
    }

    public virtual void SetParametersForShoot()
    {

    }
    #region Input Methods
    public virtual void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
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

    #region Command Methods
    [Command]
    public virtual void MoveCmd(Vector2 move)
    {
        movement.Move(move);
    }


    #endregion
}
