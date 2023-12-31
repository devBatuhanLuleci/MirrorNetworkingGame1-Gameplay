﻿using DG.Tweening;
using Mirror;
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
    public bool BasicAttackHeld { get; set; } = false;
    public bool UltiAttackHeld { get; set; } = false;

    #region States

    public enum AttackJoystickState { Up, Idle, Holding }


    public enum ShootingState { Idle, Aiming, Reloading, Cancelled }



    [SyncVar]
    public ShootingState shootingState;
    [SyncVar/*(hook =nameof(PlayAttackAnimation))*/]
    public AttackJoystickState attackJoystickState;

    [SyncVar]
    public bool isShooting;


    #endregion

    [Space(30)]
    public SplatType splatType;
    public enum SplatType { LineMissileBasic, AngleMissileBasic, BasicIndicator }

    public SplatManager Splats { get; set; }

    [HideInInspector]
    public Vector3 lookPos;


    [SerializeField]
    private Transform attackLookAtPoint;




    [HideInInspector]
    public Transform player;


    private RaycastHit hit;




    public GameObject BasicAttackBullet;
    public GameObject UltiAttackBullet;

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
    public void Targeting(Vector2 attackDirection, bool basicAttackButtonHeld = false, bool ultiAttackButtonheld = false)
    {
        if (!isLocalPlayer || isServer)
        {
            return;
        }

        if (isShooting)
        {
            //   playerController.DoSomething();
        }

        //Debug.LogError($"Targeting attackDirection: {attackDirection} attackHeld: {attackHeld}");
        //if (!netIdentity.isLocalPlayer) return;
        AttackDirection = attackDirection;

        BasicAttackHeld = basicAttackButtonHeld;
        UltiAttackHeld = ultiAttackButtonheld;



        GetDir(AttackDirection);


        //HandleAttackIndicator();
        ConfigureAttackState();
        SetLookPosition();
        RotateIndicator(AttackDirection);
        //SetBulletSpawnPointPosition();
        playerController.TargetPoint.position = player.transform.position + ((lookPos.normalized) * playerController.Range);
    }
    public void Shoot(Vector2 attackDirection)
    {


        if (attackJoystickState == AttackJoystickState.Holding)
        {
            //Shoot here!



            //Deactivate Projectile line.

            //var angle = CalculateAngle(player, attackLookAtPoint);
            //Debug.Log(angle);
            CancelAttackProjectile();
            if (!isShooting)
            {
                AttackAnimationLocalPlayer();

            }
            //Spawn the bullet object.

            Vector3 dir = new Vector3(attackDirection.x, 0, attackDirection.y);

            // playerController.playerUIHandler.CalculateProjectile(dir);
            playerController.SendAttackType(playerController.currentAttackType);

            CmdFire(false, dir);

        }

        if (attackJoystickState == AttackJoystickState.Idle)
        {
            if (shootingState == ShootingState.Cancelled)
            {

                // attackState = ShootingState.Idle;
            }
            else if (shootingState == ShootingState.Idle)
            {

                //Auto-Attack
                if (!isShooting)
                {
                    AttackAnimationLocalPlayer();

                }
                //Auto spawn bullet on current player direction.


                attackLookAtPoint.position = new Vector3(player.position.x + player.forward.x, 0f, player.position.z + player.forward.z);
              
                Vector3 dir = attackLookAtPoint.transform.position - player.transform.position;
               
             
                playerController.SendAttackType(playerController.currentAttackType);
                CmdFire(true, dir);

            }


        }

        //Reset bullet spawn point positions.
        // ResetBulletSpawnPointPosition();
        attackJoystickState = AttackJoystickState.Up;
        shootingState = ShootingState.Idle;



    }
    [Command]
    public void GetDir(Vector2 dir)
    {
        // Debug.Log("dir: " + dir);


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
                playerController.playerUIHandler.ResetProjector();
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
        if ((BasicAttackHeld || UltiAttackHeld) && AttackDirection.magnitude <= playerController.ClampedAttackJoystickOffset)
        {
            if (attackJoystickState != AttackJoystickState.Idle)
            {

                if (shootingState == ShootingState.Aiming)
                {

                    shootingState = ShootingState.Cancelled;


                }
                else if (shootingState != ShootingState.Aiming && shootingState != ShootingState.Cancelled)
                {
                    shootingState = ShootingState.Idle;

                }


                CancelAttackProjectile();
                attackJoystickState = AttackJoystickState.Idle;

            }

        }
        // If Attack Button is pressing and it is aiming.
        else if ((BasicAttackHeld || UltiAttackHeld) && AttackDirection.magnitude > playerController.ClampedAttackJoystickOffset)
        {
            if (attackJoystickState != AttackJoystickState.Holding)
            {
                ActivateIndicator();
                SelectAttackProjectile();

                shootingState = ShootingState.Aiming;
                attackJoystickState = AttackJoystickState.Holding;

            }
        }
        // If touch has released on attack button
        //else if (!(BasicAttackHeld || UltiAttackHeld) && AttackDirection.magnitude == 0)
        //{
        //    if (attackJoystickState == AttackJoystickState.Holding)
        //    {
        //        //Shoot here!



        //        //Deactivate Projectile line.

        //        //var angle = CalculateAngle(player, attackLookAtPoint);
        //        //Debug.Log(angle);
        //        CancelAttackProjectile();
        //        if (!isShooting)
        //        {
        //            AttackAnimationLocalPlayer();

        //        }
        //        //Spawn the bullet object.

        //        Vector3 dir = new Vector3(dir3.x, 0, dir3.y);

        //        // playerController.playerUIHandler.CalculateProjectile(dir);
        //        playerController.SendAttackType(playerController.currentAttackType);

        //        CmdFire(false, dir, a);

        //    }

        //    if (attackJoystickState == AttackJoystickState.Idle)
        //    {
        //        if (shootingState == ShootingState.Cancelled)
        //        {

        //            // attackState = ShootingState.Idle;
        //        }
        //        else if (shootingState == ShootingState.Idle)
        //        {

        //            //Auto-Attack
        //            if (!isShooting)
        //            {
        //                AttackAnimationLocalPlayer();

        //            }
        //            //Auto spawn bullet on current player direction.

        //            Vector3 dir = new Vector3(dir3.x, 0, dir3.y);

        //            //  playerController.playerUIHandler.CalculateProjectile(dir);
        //            playerController.SendAttackType(playerController.currentAttackType);
        //            CmdFire(true, dir, a);

        //        }


        //    }

        //    //Reset bullet spawn point positions.
        //    // ResetBulletSpawnPointPosition();
        //    attackJoystickState = AttackJoystickState.Up;
        //    shootingState = ShootingState.Idle;

        //}

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
            playerController.playerUIHandler.AttackBasicIndicator.gameObject.SetActive(true);
            playerController.playerUIHandler.AttackBasicIndicator2.gameObject.SetActive(true);
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
    private void RotateIndicator(Vector2 dir)
    {

        if (attackJoystickState == AttackJoystickState.Holding)
        {


            if (splatType == SplatType.BasicIndicator)
            {
                // BasicIndicator.RotateProjector(player, lookPos, playerController.TargetPoint, hit, Range);
                playerController.playerUIHandler.RotateProjector(player, dir, playerController.TargetPoint, hit, playerController.Range);
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
    public void CmdFire(bool isAutoattack, Vector3 lookPos)
    {

        if (!playerController.energy.HaveEnergy(isShooting) || isShooting)
        {
            return;

        }



        var normalizedDir = lookPos.normalized;
        var angle = CalculationManager.GetAngle(normalizedDir);
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
        isShooting = true;
        //playerMovement.SetPlayerRotationToTargetDirection(angle).onComplete = () =>
        //{

        //};
        RotateSpine(angle);
        //Rotate character to bullet thrown rotation and spawnBullet.
        AttackAnimationOtherClients(normalizedDir, playerController.currentAttackType);
        SetLowerBodyAnimation(normalizedDir);

        // Debug.Log(angle);


        // TODO: Multiple bullet spawn system.

        playerController.Fire(isAutoattack, lookPos);

    }

    public void RotateSpine(float angle)
    {
        playerController.RotateSpine(angle);
    }

    public void AttackAnimationLocalPlayer()
    {
        if (playerController.energy.HaveEnergy())
        {
            if (playerController.currentAttackType == PlayerController.CurrentAttackType.Basic)
            {
                playerController.PlayerAnimatorController.SetTrigger("Shoot");

            }
            else if (playerController.currentAttackType == PlayerController.CurrentAttackType.Ulti)
            {
                playerController.PlayerAnimatorController.SetTrigger("ShootUlti");
              //  playerController.CharacterSpecificStats.Handle_Specific_Object_On_Ulti_AttackButtonPressed();


            }

        }
        //   playerController.PlayerAnimatorController.Play("FatBoyFireLoopSequence");

    }
    [ClientRpc(includeOwner = false)]
    public void AttackAnimationOtherClients(Vector3 dir, PlayerController.CurrentAttackType currentAttackType)
    {

        if (currentAttackType == PlayerController.CurrentAttackType.Basic)
        {
            playerController.PlayerAnimatorController.SetTrigger("Shoot");

        }
        else if (currentAttackType == PlayerController.CurrentAttackType.Ulti)
        {
            playerController.PlayerAnimatorController.SetTrigger("ShootUlti");
         //   playerController.CharacterSpecificStats.Handle_Specific_Object_On_Ulti_AttackButtonPressed();

        }

    }
    [ClientRpc(includeOwner = true)]
    public void SetLowerBodyAnimation(Vector3 dir)
    {
        if (playerController.energy.HaveEnergy())
        {

            playerController.SetLowerBodyAnimation(dir);
            // playerController.PlayerAnimatorController.SetTrigger("Shoot");
            //  playerController.PlayerAnimatorController.Play("FatBoyFireLoopSequence");
        }
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

