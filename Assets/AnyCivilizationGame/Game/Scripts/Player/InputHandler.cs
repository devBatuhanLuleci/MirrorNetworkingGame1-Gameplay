using Mirror;
using SimpleInputNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : Singleton<InputHandler>
{
    #region Public Fields
    [HideInInspector]
    public Joystick MovementJoystick;
    [HideInInspector]
    public Joystick AttackBasicJoystick;
    [HideInInspector]
    public Joystick AttackUltiJoystick;

    public enum AttackType { Basic, Ulti }
    public AttackType attackType = AttackType.Basic;

    #endregion

    #region Private Fields
    [SerializeField]
    public PlayerController PlayerController;
    #endregion


    public void Init(PlayerController player)
    {
        PlayerController = player;
        gameObject.SetActive(true);
        JoystickCanvas joystickCanvas = GameplayPanelUIManager.Instance.joystickCanvas.GetComponent<JoystickCanvas>();

        joystickCanvas.joystickCanvasUIController.DeactivateUlti();
                MovementJoystick = joystickCanvas.MovementJoystick;
        AttackBasicJoystick = joystickCanvas.AttackBasicJoystick;
        AttackUltiJoystick = joystickCanvas.AttackUltiJoystick;
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



    public void AttackButtonUp(Joystick joystick)
    {
        if (joystick.joystickButtonType == Joystick.JoystickButtonType.ultiAttack || joystick.joystickButtonType == Joystick.JoystickButtonType.basicAttack)
        {

            var targetingDirection = Vector2.zero;

            if (joystick.joystickButtonType == Joystick.JoystickButtonType.basicAttack)
            {

                targetingDirection = AttackBasicJoystick.Value;


            }
            else if (joystick.joystickButtonType == Joystick.JoystickButtonType.ultiAttack)
            {

                targetingDirection = AttackUltiJoystick.Value;


            }
            else
            {
                targetingDirection = Vector2.zero;
            }

            PlayerController.attack.Shoot(targetingDirection);
        }
    }
    private void Attack()
    {

        var basicAttackHeld = AttackBasicJoystick.joystickHeld;
        var ultiAttackHeld = AttackUltiJoystick.joystickHeld;
        var targetingDirection = Vector2.zero;
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

        //if ((ultiAttackHeld && AttackBasicJoystick.gameObject.activeSelf) || (!ultiAttackHeld && !AttackBasicJoystick.gameObject.activeSelf))
        //    AttackBasicJoystick.gameObject.SetActive(!ultiAttackHeld);

        PlayerController.Targeting(targetingDirection, basicAttackHeld, ultiAttackHeld);

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
