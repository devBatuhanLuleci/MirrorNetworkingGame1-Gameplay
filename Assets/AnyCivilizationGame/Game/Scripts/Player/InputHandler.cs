using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : NetworkBehaviour
{
    #region Public Fields

    public NetworkIdentity NetworkIdentity { get; private set; }
    #endregion

    #region Private Fields

    private OfflineGameManager gameManager;
    private PlayerMovement PlayerMovement;
    #endregion


    private void Awake()
    {
        NetworkIdentity = GetComponent<NetworkIdentity>();
        gameManager = OfflineGameManager.Instance;

    }

    private void Start()
    {
        if (NetworkIdentity.isServer || NetworkIdentity.isLocalPlayer)
        {
            PlayerMovement = GetComponent<PlayerMovement>();
        }
    }

    private void LateUpdate()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        if (!NetworkIdentity.isLocalPlayer || NetworkIdentity.isServer) return;
        Move();
    }

    private void Move()
    {
        var moveValue = gameManager.MovementJoystick.Value;
        PlayerMovement.MovementSpriteHandler(moveValue);
        if (moveValue.sqrMagnitude >= 0f)
        {
            MoveCmd(moveValue);
        }
    }


    #region Command Methods

    [Command]
    public void MoveCmd(Vector2 move)
    {
        PlayerMovement.moveDirection = move;
    }
    #endregion
}
