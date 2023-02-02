using Mirror;
using SimpleInputNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : Singleton<InputHandler>
{
    #region Public Fields
    [SerializeField] private Joystick MovementJoystick;
    [SerializeField] private Joystick AttackBasicJoystick;
    [SerializeField] private Joystick AttackUltiJoystick;

    public enum AttackType {Basic,Ulti }
    public AttackType attackType=AttackType.Basic;
    public bool ultiActive = false;
    public bool ultideActive = false;
    #endregion

    #region Private Fields
    [SerializeField]
    public PlayerController PlayerController;
    #endregion


    public void Init(PlayerController player)
    {
        PlayerController = player;
        gameObject.SetActive(true);
    }

    protected override void Awake()
    {
        if (ACGDataManager.Instance.GameData.TerminalType == TerminalType.Server)
        {
            Destroy(gameObject);
            return;
        }

        base.Awake();
        gameObject.SetActive(false);


      


    }
    private void Start()
    {
        //if (attackType == AttackType.Basic)
        //{
        //    AttackUltiJoystick.Activate();
        //}
    }
    private void Update()
    {
        if (PlayerController == null || !PlayerController.IsLive) return;
        Move();

        //if (ultiActive)
        //{
        //    ultiActive = false;
        //    if (attackType == AttackType.Basic)
        //    {
        //        AttackUltiJoystick.Activate();
        //    }
        //}
        //if (ultideActive)
        //{
        //    ultideActive = false;
        //    if (attackType == AttackType.Basic)
        //    {
        //        AttackUltiJoystick.Deactivate();
        //    }
        //}
        //  BasicAttack();
        //   UltiAttack();
        Attack();
    }
    private void Attack()
    {
       
        var basicAttackHeld = AttackBasicJoystick.joystickHeld;
        var ultiAttackHeld = AttackUltiJoystick.joystickHeld;
        var targetingDirection =Vector2.zero;
        if (basicAttackHeld)
        {
            
            targetingDirection = AttackBasicJoystick.Value;


        }
        else if (ultiAttackHeld)
        {

            targetingDirection = AttackUltiJoystick.Value;

           
        }
        else
        {
            targetingDirection = Vector2.zero;
        }

        if ((ultiAttackHeld && AttackBasicJoystick.gameObject.activeSelf) || (!ultiAttackHeld && !AttackBasicJoystick.gameObject.activeSelf))
            AttackBasicJoystick.gameObject.SetActive(!ultiAttackHeld);

        PlayerController.Targeting(targetingDirection, basicAttackHeld,ultiAttackHeld);

    }

  
    private void Move()
    {
        var moveValue = MovementJoystick.Value;
        PlayerController.Move(moveValue);
    }

    //public void CalculateSomething()
    //{
    //    //   Debug.Log("moveDir " + MovementJoystick.Value);
    //    //   Debug.Log("attackDir " + AttackJoystick.Value);
    //    var direction = target.position - transform.position;
    //    direction.Normalize();

    //    var offsetVector = Vector3.Cross(Vector2.up, direction);
    //    offsetVector.Normalize();
    //    var startPosition = transform.position + offsetVector * localHorizontalOffset + direction * radialOffset;


    //}



}
