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
    private bool isInitilized=false;
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
        isInitilized = true;
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

    }

    private void Update()
    {
        //NetworkedGameManager.Instance.IsGameStarted();
        //      NetworkedGameManager.Instance.IsClientConnected();
        if (NetworkedGameManager.Instance == null) { return; }
        if (!NetworkedGameManager.Instance.isClientConnected) { return; }
        if (PlayerController == null || !PlayerController.IsLive || !NetworkedGameManager.Instance.isGameStarted || NetworkedGameManager.Instance.isGameFinished) return;

        Move();
        Attack();

    
}



       
    





    public void AttackButtonUp(Joystick joystick)
{
        //if (NetworkedGameManager.Instance == null) { return; }
        //if (!NetworkedGameManager.Instance.isClientConnected) { return; }
        if (PlayerController == null || !PlayerController.IsLive || NetworkedGameManager.Instance.isGameFinished) return;

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

    PlayerController.Targeting(targetingDirection, basicAttackHeld, ultiAttackHeld);

}


private void Move()
{
    var moveValue = MovementJoystick.Value;
    PlayerController.Move(moveValue);
}




}
