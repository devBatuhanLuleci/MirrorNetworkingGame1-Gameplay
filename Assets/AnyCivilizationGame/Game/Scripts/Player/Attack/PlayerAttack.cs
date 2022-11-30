using DG.Tweening;
using Mirror;
using PredictedProjectileExample;
using SimpleInputNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Werewolf.StatusIndicators.Components;

public class PlayerAttack : NetworkBehaviour
{

    private PlayerController playerController;
    //private Joystick attackJoystick;
    public Vector2 AttackDirection { get; set; } = Vector2.zero;
    public bool AttackHeld { get; set; } = false;

    #region States

    public enum AttackJoystickState { Up, Idle, Holding }

    public enum ShootingState { Idle, Aiming, Reloading, Shooting, Cancelled }



    public ShootingState attackState;
    [SyncVar/*(hook =nameof(PlayAttackAnimation))*/]
    public AttackJoystickState attackJoystickState;



    #endregion

    [Space(30)]
    public SplatType splatType;
    public enum SplatType { LineMissileBasic, AngleMissileBasic, BasicIndicator }

    public SplatManager Splats { get; set; }

    [SerializeField]
    private BasicIndicator BasicIndicator;

    public float Range = 3.6f;

    [HideInInspector]
    public Vector3 lookPos;


    [SerializeField]
    private Transform attackLookAtPoint;


    private float ClampedAttackJoystickOffset = 0.1f;

    [HideInInspector]
    public Transform player;


    private RaycastHit hit;




    public GameObject Bullet;

    private PlayerMovement playerMovement;


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void Start()
    {
        //if (!netIdentity.isLocalPlayer) return;
        InitilizeVariables();
        ActivateIndicator();
    }
    public void Targeting(Vector2 attackDirection, bool attackHeld = false)
    {
        //Debug.LogError($"Targeting attackDirection: {attackDirection} attackHeld: {attackHeld}");
        //if (!netIdentity.isLocalPlayer) return;
        AttackDirection = attackDirection;
        AttackHeld = attackHeld;
        ConfigureAttackState();
        SetLookPosition();
        RotateIndicator();
        //SetBulletSpawnPointPosition();
        playerController.TargetPoint.position = player.transform.position + ((lookPos.normalized) * Range);
    }

    /// <summary>
    /// We initilize some variables in the begining.
    /// </summary>
    public void InitilizeVariables()
    {
        InitilizeClientVariables();
        playerMovement = GetComponent<PlayerMovement>();
        player = this.gameObject.transform;




    }

    private void InitilizeClientVariables()
    {
        Splats = GetComponentInChildren<SplatManager>(true);
    }

    public void CancelAttackProjectile()
    {
        Splat attackSplat = Splats.CurrentSpellIndicator;
        switch (splatType)
        {
            case SplatType.LineMissileBasic:

                if (attackSplat.Manager != null)
                {
                    attackSplat.Manager.CancelSpellIndicator();
                    attackSplat.ChangeTransparency(0);
                }

                break;
            case SplatType.AngleMissileBasic:
                if (attackSplat.Manager != null)
                {
                    attackSplat.Manager.CancelSpellIndicator();
                    attackSplat.ChangeTransparency(0);
                }

                break;

            case SplatType.BasicIndicator:
                BasicIndicator.ResetProjector();
                break;

            default:
                break;
        }


    }
    public void SelectAttackProjectile()
    {

        Splat attackSplat = Splats.CurrentSpellIndicator;
        switch (splatType)
        {
            case SplatType.LineMissileBasic:
                if (attackSplat.Manager != null)
                {
                    attackSplat.ChangeTransparency(attackSplat.transparencyValue);
                }
                break;

            case SplatType.AngleMissileBasic:
                if (attackSplat.Manager != null)
                {
                    attackSplat.ChangeTransparency(attackSplat.transparencyValue);
                }
                break;

            case SplatType.BasicIndicator:
                break;

            default:
                break;
        }


    }

    public void ConfigureAttackState()
    {
        // If Attack Button is pressing and it is not aiming.
        if (AttackHeld && AttackDirection.sqrMagnitude <= ClampedAttackJoystickOffset)
        {
            if (attackJoystickState != AttackJoystickState.Idle)
            {

                if (attackState == ShootingState.Aiming)
                {

                    attackState = ShootingState.Cancelled;
                }
                else if (attackState != ShootingState.Aiming && attackState != ShootingState.Cancelled)
                {
                    attackState = ShootingState.Idle;

                }


                CancelAttackProjectile();

                attackJoystickState = AttackJoystickState.Idle;

            }

        }
        // If Attack Button is pressing and it is aiming.
        else if (AttackHeld && AttackDirection.sqrMagnitude > ClampedAttackJoystickOffset)
        {
            if (attackJoystickState != AttackJoystickState.Holding)
            {
                attackState = ShootingState.Aiming;
                attackJoystickState = AttackJoystickState.Holding;
                ActivateIndicator();
                SelectAttackProjectile();
            }
        }
        // If touch has released on attack button
        else if (!AttackHeld && AttackDirection.sqrMagnitude == 0)
        {
            if (attackJoystickState == AttackJoystickState.Holding)
            {
                //Shoot here!

                attackState = ShootingState.Shooting;

                //Deactivate Projectile line.
                CancelAttackProjectile();
                //var angle = CalculateAngle(player, attackLookAtPoint);
                //Debug.Log(angle);
                AttackAnimationLocalPlayer();
                //Spawn the bullet object.

                var startPos = player.transform.position + ((lookPos.normalized) );
                var targetPos = player.transform.position + ((lookPos.normalized) * 2);

                var direction = targetPos - startPos;

                var finalDir = new Vector3(direction.x, 0, direction.z).normalized;


                var dir = finalDir;

                playerController.playerUIHandler.CalculateProjectile(dir);

                CmdFire(false,

                  dir,


                    playerController.playerUIHandler.v0,
                    playerController.playerUIHandler.angle,
                    playerController.playerUIHandler.timeNew,
                    playerController.initialVelocity);
                //Debug.Log("stat1:"+ playerController.playerUIHandler.groundDirection.normalized +
                //" stat2:" + playerController.playerUIHandler.v0
                //+ " stat3:" + playerController.playerUIHandler.angle +
                //" stat4:" + playerController.playerUIHandler.timeNew +
                //" stat5:" + playerController.initialVelocity);
            }

            if (attackJoystickState == AttackJoystickState.Idle)
            {
                if (attackState == ShootingState.Cancelled)
                {

                    attackState = ShootingState.Idle;
                }
                else if (attackState == ShootingState.Idle)
                {
                    //Auto-Attack
                    attackState = ShootingState.Shooting;
                    AttackAnimationLocalPlayer();
                    //Auto spawn bullet on current player direction.

                    var startPos = player.transform.position + ((lookPos.normalized));
                    var targetPos = player.transform.position + ((lookPos.normalized) * 2);

                    var direction = targetPos - startPos;

                    var finalDir = new Vector3(direction.x, 0, direction.z).normalized;


                    var dir = finalDir;

                    playerController.playerUIHandler.CalculateProjectile(dir);

                    CmdFire(false,

                      dir,


                        playerController.playerUIHandler.v0,
                        playerController.playerUIHandler.angle,
                        playerController.playerUIHandler.timeNew,
                        playerController.initialVelocity);

                }


            }

            //Reset bullet spawn point positions.
            // ResetBulletSpawnPointPosition();
            attackJoystickState = AttackJoystickState.Up;

        }

    }


    /// <summary>
    /// Calculate angle between two vectors in 360 degrees.
    /// </summary>
    /// <param name="from initposition"></param>
    /// <param name="to targetposition"></param>
    /// <returns></returns>

    /// <summary>
    /// Activate the indicator.
    /// </summary>
    private void ActivateIndicator()
    {
        if (splatType == SplatType.BasicIndicator)
        {
            BasicIndicator.gameObject.SetActive(true);
        }
        else
        {
            Splats.SelectSpellIndicator(splatType.ToString());
        }
    }
    private void SetLookPosition()
    {
        if (attackJoystickState == AttackJoystickState.Holding)
        {
            attackLookAtPoint.position = new Vector3(AttackDirection.x + player.position.x, 0f, AttackDirection.y + player.position.z);
        }
        else
        {
            attackLookAtPoint.position = new Vector3(player.position.x + player.forward.x, 0f, player.position.z + player.forward.z);
        }
        Vector3 targetDir = attackLookAtPoint.transform.position - player.transform.position;
        lookPos = targetDir;
        lookPos.y = 0;


    }

    /// <summary>
    /// This function rotates the indicator object.
    /// </summary>
    private void RotateIndicator()
    {

        if (attackJoystickState == AttackJoystickState.Holding)
        {


            if (splatType == SplatType.BasicIndicator)
            {
                BasicIndicator.RotateProjector(player, lookPos, playerController.TargetPoint, hit, Range);
            }
            else
            {
                if (Splats.CurrentSpellIndicator.Manager != null)
                {

                    Splats.CurrentSpellIndicator.Manager.transform.DORotateQuaternion(Quaternion.LookRotation(lookPos), 0f).SetEase(Ease.Linear);

                }

            }


        }

    }

    /// <summary>
    /// This function spawns bullet and throw with some informations.
    /// </summary>
    /// 
    [Command]
    public void CmdFire(bool isAutoattack,Vector3 dir, float speed, float angleNew, float timeNew, float initialVelocity)
    {
         var angle = CalculationManager.GetAngle(dir);
        //Debug.Log("angle "+ CalculateAngle(BasicIndicator.AttackBasicIndicator.GetPosition(0), BasicIndicator.AttackBasicIndicator.GetPosition(1)));
   
        // Debug.Log("değer : "+ CalculateAngle(player, dir));
        #region MultipleBullet

        //foreach (BulletSpawnPoint BulletSpawnPoint in BulletSpawnPoints)
        //{
        //    var offsetVector = Vector3.Cross(Vector3.up, lookPos.normalized);
        //    offsetVector.Normalize();


        //    var BulletTargetOffSetZ = Vector3.Distance(new Vector3( player.position.x + offsetVector.x * BulletSpawnPoint.BulletInitPos.x, 0f,player.position.z + offsetVector.z * BulletSpawnPoint.BulletInitPos.x) , new Vector3( BulletSpawnPoint.spawnPoint.position.x,0f, BulletSpawnPoint.spawnPoint.position.z));
        //    Debug.Log(BulletTargetOffSetZ);
        //objectPooler.SpawnFromPool("Bullet", BulletSpawnPoint.spawnPoint.position- new Vector3(0,0.4f,0f), transform.rotation, this, CalculateAngle(player, attackLookAtPoint), BulletTargetOffSetZ);

        //}
        // Debug.Log(Bullet.transform.name + " " + objectPooler.pools[0].tag);
        #endregion

        playerMovement.SetPlayerRotationToTargetDirection(angle).onComplete = () =>
        {

        };
        //Rotate character to bullet thrown rotation and spawnBullet.
        AttackAnimationOtherClients();
 
        // Debug.Log(angle);
    



        playerController.SpawnBullet(isAutoattack, dir, speed, angleNew, timeNew, initialVelocity);
    
    }   


    public void AttackAnimationLocalPlayer()
    {

        playerController.PlayerAnimatorController.SetTrigger("Shoot");

    }
    [ClientRpc(includeOwner = false)]
    public void AttackAnimationOtherClients()
    {

        playerController.PlayerAnimatorController.SetTrigger("Shoot");

    }




    /// <summary>
    /// This function handles multiple bullet position on player.
    /// </summary>ö
    //    private void SetBulletSpawnPointPosition()
    //    {

    //        if (attackJoystickState == AttackJoystickState.Holding)
    //        {

    //            if (splatType == SplatType.BasicIndicator)
    //            {

    //                var offsetVector = Vector3.Cross(Vector3.up, lookPos.normalized);
    //                offsetVector.Normalize();

    //                foreach (BulletSpawnPoint BulletSpawnPoint in BulletSpawnPoints)
    //                {

    //                    BulletSpawnPoint.spawnPoint.eulerAngles = new Vector3(0, CalculateAngle(player, attackLookAtPoint), 0);
    //                    var BulletPosition = new Vector3(player.transform.position.x + (lookPos.normalized.x * BulletSpawnPoint.BulletInitPos.z) + offsetVector.x * BulletSpawnPoint.BulletInitPos.x,
    //                                                             BulletSpawnPoint.spawnPoint.position.y,
    //                                                             player.transform.position.z + (lookPos.normalized.z * BulletSpawnPoint.BulletInitPos.z) + offsetVector.z * BulletSpawnPoint.BulletInitPos.x);
    //                    BulletSpawnPoint.spawnPoint.position = BulletPosition;
    //                }
    //            }
    //        }

    //    }
    //    /// <summary>
    //    /// Reset bullet spawn point positions for auto attack bullet position.
    //    /// </summary>
    //    private void ResetBulletSpawnPointPosition()
    //    {

    //        if (splatType == SplatType.BasicIndicator)
    //        {
    //            var offsetVector = Vector3.Cross(Vector3.up, lookPos.normalized);
    //            offsetVector.Normalize();

    //            foreach (BulletSpawnPoint BulletSpawnPoint in BulletSpawnPoints)
    //            {
    //                BulletSpawnPoint.spawnPoint.localEulerAngles = BulletSpawnPoint.BulletInitRot;
    //                BulletSpawnPoint.spawnPoint.localPosition = BulletSpawnPoint.BulletInitPos;
    //            }
    //        }

    //    }


}

