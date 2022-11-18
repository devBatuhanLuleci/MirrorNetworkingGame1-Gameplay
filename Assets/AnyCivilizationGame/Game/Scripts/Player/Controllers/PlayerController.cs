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
    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
    }
    #region Input Methods
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
