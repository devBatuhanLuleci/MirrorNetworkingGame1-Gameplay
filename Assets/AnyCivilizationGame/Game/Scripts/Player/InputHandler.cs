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
    [SerializeField] private Joystick AttackJoystick;
    #endregion

    #region Private Fields
    [SerializeField]
    private PlayerController PlayerController;
    #endregion


    public void Init(PlayerController player)
    {
        PlayerController = player;
        gameObject.SetActive(true);
    }

    protected override void Awake()
    {
        if (ACGDataManager.Instance.GameData.IsServer)
        {
            Destroy(gameObject);
            return;
        }

        base.Awake();
        gameObject.SetActive(false);

    }
    private void Update()
    {
        if (PlayerController == null || !PlayerController.IsLive) return;
        Move();
        MainAttack();
       // CalculateSomething();
    }

    private void MainAttack()
    {
        var targetingDirection = AttackJoystick.Value;
        var held = AttackJoystick.joystickHeld;
        PlayerController.Targeting(targetingDirection, held);

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
