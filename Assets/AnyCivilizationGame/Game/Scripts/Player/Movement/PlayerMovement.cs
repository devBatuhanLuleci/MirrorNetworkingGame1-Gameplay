
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
    public MovementState movementState => MoveToDirection != Vector3.zero ? MovementState.Moving : MovementState.Idle;
    #endregion


    [SyncVar]
    public Vector3 MoveToDirection;

    [SyncVar(hook = nameof(RotateSpine))]
    public float angle = 0;

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
    private Rigidbody rb;

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        SetSpriteVisibility(false);

    }
    private void Update()
    {
        if (netIdentity.isServer)
        {
            SetPlayerPosition();
            SetPlayerRotation();

        }
        else
        {
            SetCurrentAnimation();
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
        else if (movementState == MovementState.Moving)
        {
            PlayerController.PlayerAnimatorController?.SetBool("Running", true);
        }
    }





    /// <summary>
    /// This function moves the character.
    /// </summary>
    public void SetPlayerPosition()
    {
        if (movementState == MovementState.Moving)
        {
            var newPos = transform.position + movementSpeed * Time.deltaTime * MoveToDirection;
            rb.position = newPos;
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.Sleep();
        }
    }

    /// <summary>
    /// This function rotate the character.
    /// </summary>
    public void SetPlayerRotation()
    {
        if (movementState == MovementState.Idle /*|| PlayerController.attack.isShooting*/)
            return;
        var rotation = Quaternion.LookRotation(MoveToDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
    public Tween SetPlayerRotationToTargetDirection(float targetPos)
    {

        return transform.DORotateQuaternion(Quaternion.Euler(transform.rotation.eulerAngles.x, targetPos, transform.rotation.eulerAngles.z), rotationTurnSpeed).SetEase(Ease.InOutQuad);
    }


    public void RotateSpine(float oldAngle, float newAngle)
    {
        // Debug.Log("newAngle : " + newAngle) ;
        //   PlayerController.SpineRotator.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(newAngle, newAngle, newAngle)), .1f).SetEase(Ease.InOutQuad);
        var LocalAngle = transform.rotation.eulerAngles.y + PlayerController.SpineRotator.rotation.eulerAngles.x - newAngle;

        PlayerController.SpineRotator.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(LocalAngle, 0, 0)), PlayerController.AttackTurnSpeed).SetEase(Ease.InOutQuad);
    }
    public void GetCurrentRotateSpine(float angle)
    {

        //   PlayerController.SpineRotator.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(newAngle, newAngle, newAngle)), .1f).SetEase(Ease.InOutQuad);
        var LocalAngle = transform.rotation.eulerAngles.y + PlayerController.SpineRotator.rotation.eulerAngles.x - angle;

        PlayerController.SpineRotator.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(LocalAngle, 0, 0)), 0f).SetEase(Ease.InOutQuad);
    }
    public void RotateSpineReset()
    {

        PlayerController.SpineRotator.DOLocalRotateQuaternion(Quaternion.Euler(Vector3.zero), PlayerController.AttackTurnSpeed).SetEase(Ease.InOutQuad);

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
}

