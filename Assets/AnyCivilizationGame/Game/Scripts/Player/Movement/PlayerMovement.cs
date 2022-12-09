using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInputNamespace;
using System;
using Mirror;
using DG.Tweening;

public class PlayerMovement : NetworkBehaviour
{
    #region MovementState
    public enum MovementState { Idle, Moving }
    [SyncVar]
    public MovementState movementState;
    #endregion


    [SyncVar]
    public Vector2 moveDirection;





    [SerializeField]
    private Transform playerDirSprite;


    [SerializeField]
    private float rotationSpeed = 8f;

    [SerializeField]
    private float rotationTurnSpeed = .4f;


    [SerializeField]
    private float movementSpeed = 5f;


    [SerializeField]
    private float DirectionSpriteScale = 2f;

    private PlayerController PlayerController;

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
    }
    private void Start()
    {
        SetSpriteVisibility(false);

    }
    private void Update()
    {
        if (netIdentity.isServer)
        {
            MovementStateHandler();
            SetPlayerPosition();
            SetPlayerRotation();

        }
        else
        {
            SetCurrentAnimation();
        }
        ResetMovementInput();
    }

    /// <summary>
    /// Reset movement input if player not send input on the network
    /// </summary>
    private void ResetMovementInput()
    {
        if (moveDirection.sqrMagnitude > 0)
        {
            moveDirection = moveDirection * 0.5f;
        }
        else
        {
            moveDirection = Vector2.zero;
        }

    }

    public void MovementSpriteHandler(Vector2 moveInput)
    {
        SetSpriteVisibility(movementState == MovementState.Moving);
        SetDirSpritePosition(moveInput);
    }


    /// <summary>
    /// This function is setting the animation.
    /// </summary>
    private void SetCurrentAnimation()
    {
        if (movementState == MovementState.Idle)
        {
            PlayerController.PlayerAnimatorController?.SetBool("Running", false);
        }
        else if (movementState == MovementState.Moving )
        {
            PlayerController.PlayerAnimatorController?.SetBool("Running", true);
        }
    }

    /// <summary>
    /// This function setting the movement state, setting sprite visibility and set current animation.
    /// </summary>
    public void MovementStateHandler()
    {
        if (moveDirection.sqrMagnitude < 0.1f)
        {
            if (movementState != MovementState.Idle)
            {
                movementState = MovementState.Idle;            
            }
        }
        else
        {
            if (movementState != MovementState.Moving)
            {
                movementState = MovementState.Moving;               
            }
        }
    }




    /// <summary>
    /// This function moves the character.
    /// </summary>
    public void SetPlayerPosition()
    {
        if (movementState == MovementState.Moving)
        {
            Vector3 dir = new Vector3(moveDirection.x, 0, moveDirection.y).normalized;
            transform.Translate(dir * movementSpeed, Space.World);

        }
    }

    /// <summary>
    /// This function rotate the character.
    /// </summary>
    public void SetPlayerRotation()
    {
        if (movementState == MovementState.Idle  || PlayerController.attack.attackState==PlayerAttack.ShootingState.Shooting)
            return;

        var lookPos = new Vector3(moveDirection.x, 0f, moveDirection.y).normalized;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
    public Tween SetPlayerRotationToTargetDirection(float targetPos)
    {
       
        return transform.DORotateQuaternion(Quaternion.Euler(transform.rotation.eulerAngles.x, targetPos, transform.rotation.eulerAngles.z), rotationTurnSpeed).SetEase(Ease.InOutQuad);
    }

    /// <summary>
    /// This function sets PlayerDirSprite Position.
    /// </summary>
    public void SetDirSpritePosition(Vector2 moveInput)
    {
        var scale = new Vector3(moveInput.x, 0, moveInput.y) / DirectionSpriteScale;
        playerDirSprite.position = scale + transform.position;
    }
    /// <summary>
    /// This function sets PlayerDirSprite Visibility.
    /// </summary>
    /// <param name="visiblityState"  takes visibility state.></param>
    public void SetSpriteVisibility(bool visiblityState)
    {
        playerDirSprite.gameObject.SetActive(visiblityState);
    }

    public void Move(Vector2 moveInput)
    {
        moveDirection = moveInput;
    }






}

