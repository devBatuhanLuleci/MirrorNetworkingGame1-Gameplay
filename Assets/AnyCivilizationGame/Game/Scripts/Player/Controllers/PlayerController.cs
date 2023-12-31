﻿using DG.Tweening;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : ObjectController
{

    #region     Private Fields
    private PlayerMovement movement;
    [HideInInspector]
    public PlayerAttack attack;


    
    internal Energy energy;


    internal UltimateSkill ultimateSkill;

    private InfoPopup infoPopup;
    #endregion


    public Transform GemCollectPoint;
    public Transform SpineRotator;
    internal PlayerUIHandler playerUIHandler;
    internal Animator PlayerAnimatorController;
    public CharacterSpecificStats CharacterSpecificStats;

    public bool isUltiThrowable = false;



    public enum DamageTakenStatus { Idle, IsTakenDamage, Healing }
    [SyncVar]
    public DamageTakenStatus damageTakenStatus;

    private Coroutine damageTakenCoroutine;

    public float AttackTurnSpeed = 0.25f;
    public float ClampedAttackJoystickOffset = 0.005f;
    public List<BulletSpawnPoints> BulletSpawnPoints;
    private Vector3 attackDir;
    #region Character Projectile Details

    public Transform TargetPoint;

    public float Range = 5f;
    public float radialOffset = .6f;

    public int BulletCount = 1;
    public float BulletIntervalTime = .2f;

    public float StartFireBasickAttackAnimationWaitTime = .2f;
    public float FinishFireAnimationWaitTime = .2f;

    public float StartFireUltiAttackAnimationWaitTime = 1f;

    public enum BasicAttackType { Straight, Parabolic }
    public BasicAttackType basicAttackType;


    public enum UltiAttackType { Straight, Parabolic }
    public UltiAttackType ultiAttackType;


    public enum CurrentAttackType { Basic, Ulti }

    public CurrentAttackType currentAttackType;


    #endregion




    private float TrailDistance;


    public override void Awake()
    {
        base.Awake();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        energy = GetComponent<Energy>();
        playerUIHandler = GetComponent<PlayerUIHandler>();
        ultimateSkill = GetComponent<UltimateSkill>();



    }
    private void Start()
    {
        foreach (BulletSpawnPoints spawnPoint in BulletSpawnPoints)
        {
            spawnPoint.BulletInitPos = spawnPoint.spawnPoint;
        }


    }

    public virtual void DetectJoystickButton(SimpleInputNamespace.Joystick.JoystickButtonType joystickButtonType)
    {

        switch (joystickButtonType)
        {
            case SimpleInputNamespace.Joystick.JoystickButtonType.movement:

                break;

            case SimpleInputNamespace.Joystick.JoystickButtonType.ultiAttack:

                //  (int)ultiAttackType;

                playerUIHandler.projectileType = (PlayerUIHandler.ProjectileType)(int)ultiAttackType;
                currentAttackType = CurrentAttackType.Ulti;

                break;
            case SimpleInputNamespace.Joystick.JoystickButtonType.basicAttack:


                playerUIHandler.projectileType = (PlayerUIHandler.ProjectileType)(int)basicAttackType;
                currentAttackType = CurrentAttackType.Basic;


                break;
            default:
                break;


        }

    }


    public void SpawnBullet(Vector3[] spawnPoint, Vector3 dir, int BulletCount, float BulletIntervalTime, float offSetZvalue = 0, float offSetYvalue = 0)
    {
        var lobbyPlayer = ACGDataManager.Instance.LobbyPlayer;
        energy.CastEnergy();
        StartCoroutine(SpawnIntervalBullet(spawnPoint, dir, BulletCount, BulletIntervalTime, offSetZvalue, offSetYvalue));


    }

    public void ChangeDamageTakenStatus(DamageTakenStatus damageTakenStatus)
    {
        this.damageTakenStatus = damageTakenStatus;
        switch (damageTakenStatus)
        {
            case DamageTakenStatus.Idle:
                {


                    break;
                }
            case DamageTakenStatus.IsTakenDamage:
                {

                    var playerHealth = health as PlayerHealth;
                    playerHealth.StopHealthIncreaseCoroutine();
                    damageTakenCoroutine = StartCoroutine(WaitForHealingTime());
                    break;
                }
            case DamageTakenStatus.Healing:

                {

                    var playerHealth = health as PlayerHealth;
                    playerHealth.IncreaseHealthOverTime();
                    break;
                }
            default:
                break;
        }

    }
    public IEnumerator WaitForHealingTime()
    {




        if (damageTakenCoroutine != null)
        {
            StopCoroutine(damageTakenCoroutine);

        }

        yield return new WaitForSeconds(4f);

        if (IsLive)
        {
            ChangeDamageTakenStatus(DamageTakenStatus.Healing);

        }
        //else
        //{
        //    ChangeDamageTakenStatus(DamageTakenStatus.Idle);

        //}

    }


    public override void TakeDamage(int damage, NetworkConnection target)
    {


        base.TakeDamage(damage, target);
        OnTakeDamage_DoSomething_Only_On_Other_Client(target);
        OnTakeDamage_DoSomething_On_Clients();
    }
    //[Command(requiresAuthority =false)]
    //public void wow()
    //{
    //    Debug.Log("server'da çalıştım");
    //    CallPlease();
    //}

    //[ClientRpc]
    //public void CallPlease()
    //{
    //    Debug.Log("client'a mesaj verdim.");
    //}


    [ClientRpc]
    public void OnTakeDamage_DoSomething_On_Clients()
    {
        playerUIHandler.AnimateOtherHealthBarEffects();
        //  playerUIHandler.Color_Switch_On_Health_Change(health.HealthRate);

    }
    [TargetRpc]
    public void OnTakeDamage_DoSomething_Only_On_Other_Client(NetworkConnection target)
    {
        // playerUIHandler.AnimateOtherHealthBarEffects();
        playerUIHandler.Color_Switch_On_Health_Change(health.HealthRate);

    }

    [TargetRpc]
    public void OnRevive_DoSomething_Only_On_This_Client(NetworkConnection target)
    {
        //   Debug.Log("SANA BİLGİ MESAJI VERDİM.");
        // playerUIHandler.AnimateOtherHealthBarEffects();
        playerUIHandler.Reset_HealthBar(true);
        //  playerUIHandler
    }
    [TargetRpc]
    public void OnCurrentHealthReachedMaxHealth(NetworkConnection target)
    {

        playerUIHandler.Reset_HealthBar(false);

    }
    [Command]
    public void SendAttackType(CurrentAttackType currentAttackType)
    {

        if (!energy.HaveEnergy(attack.isShooting) || attack.isShooting)
        {
            return;

        }


        if (currentAttackType == CurrentAttackType.Ulti)
        {
            if (!isUltiThrowable)
            {
                //Hack detected.
                return;
            }

        }

        this.currentAttackType = currentAttackType;


    }

    //public void SpawnIntervalBullet(int BulletCount, float BulletIntervalTime)
    //{
    //    var time = BulletIntervalTime;
    //    int currentBulletCount = BulletCount;
    //    while (currentBulletCount > 0)
    //    {
    //        time -= Time.unscaledDeltaTime;
    //        Debug.Log("time : " + time);
    //        if (time <= 0f)
    //        {

    //            time = BulletIntervalTime;
    //            currentBulletCount--;
    //            Debug.Log($"Shoot, currentBulletCount:{currentBulletCount}");
    //        }
    //    }

    //}
    [ClientRpc]
    public void OnCrystalCollected_UpdatePlayer(int crystalAmount)
    {
        playerUIHandler.ActivatePlayerCrystalPanel();
        playerUIHandler.HandlePlayerCrystalAmountText(crystalAmount);
        //  playerUIHandler.Color_Switch_On_Health_Change(health.HealthRate);

    }
    [ClientRpc]
    public void OnCrystalRemoved_UpdatePlayer(int crystalAmount)
    {
        playerUIHandler.DisablePlayerCrystalPanel();
        playerUIHandler.HandlePlayerCrystalAmountText(crystalAmount);
        //  playerUIHandler.Color_Switch_On_Health_Change(health.HealthRate);

    }


    public virtual void OnBulletObjectSpawned(Throwable obj)
    {

        obj.OnObjectSpawn();

    }
    public virtual void Activate_Something_OnBulletObjectSpawned_Before()
    {
        //Inherited


    }
    public Vector3 StartPosOffSet2(Vector3 dir, float radialOffSet)
    {

        var direction = new Vector3(dir.x, 0, dir.z);
        direction.Normalize();

        var offsetVector = Vector3.Cross(Vector3.up, direction);
        offsetVector.Normalize();
        var startPosition = direction * (-radialOffSet);


        return startPosition;


    }
    public void ResetUltiStats()
    {
        isUltiThrowable = false;

        ultimateSkill.ResetCurrentFillAmount(netIdentity.connectionToClient);
        DeactivateUltiUI(netIdentity.connectionToClient);
    }
    public IEnumerator SpawnIntervalBullet(Vector3[] spawnPoint, Vector3 dir, int BulletCount, float BulletIntervalTime, float offSetZvalue = 0, float offSetYvalue = 0)
    {

        Activate_Something_OnBulletObjectSpawned_Before();


        if (currentAttackType == CurrentAttackType.Ulti)
        {
            yield return new WaitForSeconds(StartFireUltiAttackAnimationWaitTime);
        }
        else if
        (currentAttackType == CurrentAttackType.Basic)
        {
            yield return new WaitForSeconds(StartFireBasickAttackAnimationWaitTime);
        }



        int currentBulletCount = BulletCount;

        while (currentBulletCount > 0)
        {
            for (int i = 0; i < spawnPoint.Length; i++)
            {
                SetShootingParameter(true);
                yield return new WaitForSeconds(BulletIntervalTime);








                string name = "";
                Vector3 pos = Vector3.zero;
                Vector3 tempDir = Vector3.zero;
                if (currentAttackType == CurrentAttackType.Ulti)
                {


                    ResetUltiStats();
                    var direction = new Vector3(dir.x, 0, dir.z);
                    direction.Normalize();


                    //tempDir = new Vector3(dir.x, 0, dir.z);

                    //tempDir.Normalize();

                    name = attack.UltiAttackBullet.transform.name;
                    pos = transform.position
                                           + (direction * spawnPoint[i % spawnPoint.Length].x)
                                            + (Vector3.up * spawnPoint[i % spawnPoint.Length].y)
                                            + (direction * spawnPoint[i % spawnPoint.Length].z);



                }
                else if (currentAttackType == CurrentAttackType.Basic)
                {



                    var offsetVector = Vector3.Cross(Vector3.up, dir);
                    offsetVector.Normalize();

                    //tempDir = Vector3.Cross(Vector3.up, dir);
                    //tempDir.Normalize();


                    name = attack.BasicAttackBullet.transform.name;
                    pos = transform.position
                        + offsetVector * spawnPoint[i % spawnPoint.Length].x
                        + transform.up * spawnPoint[i % spawnPoint.Length].y
                        + dir * (spawnPoint[i % spawnPoint.Length].z);
                    // pos = transform.position + direction * spawnPoint[i % spawnPoint.Length].x + transform.up * spawnPoint[i % spawnPoint.Length].y + direction * (spawnPoint[i % spawnPoint.Length].z );
                }


                var spawnedBullet = ObjectPooler.Instance.Get(name, pos, Quaternion.Euler(0, CalculationManager.GetAngle(dir), 0)).GetComponent<Throwable>();

                spawnedBullet.Init("Debug User " + netId, netId, netIdentity.connectionToClient.connectionId, 0, true);


                tempDir = new Vector3(dir.x, 0, dir.z);

                tempDir.Normalize();

                var targetPos = new Vector3(pos.x, 0, pos.z);
                var playerPos = new Vector3(transform.position.x, 0, transform.position.z);


                var dirToTarget = targetPos - playerPos;

                //dir yerine tempDir olabilir. 
                var Dot = Vector3.Dot(tempDir, dirToTarget);

                var ex = tempDir * Dot;



                //var relativePos = transform.InverseTransformPoint(playerPos + ex);
                var value = 0f;



                if (Dot > 0)
                {

                    value = -ex.magnitude;


                }
                else
                {


                    value = Mathf.Abs(-ex.magnitude);

                }

                var simpleZOffSet = value;

                var startPosition = playerPos + ex/*+ offsetVector * localHorizontalOffset + direction * radialOffset*/;



                spawnedBullet.Throw(dir, Range, offSetZvalue + simpleZOffSet, offSetYvalue, radialOffset);
                NetworkServer.Spawn(spawnedBullet.gameObject);
                OnBulletObjectSpawned(spawnedBullet);

                currentBulletCount--;
                // Debug.Log($"Shoot, currentBulletCount:{currentBulletCount}");

            }

        }
        yield return new WaitForSeconds(FinishFireAnimationWaitTime);
        attack.isShooting = false;
        attack.shootingState = PlayerAttack.ShootingState.Idle;
        RotateSpineResetter();
        SetShootingParameter(false);

    }
    [ClientRpc]
    public void SetShootingParameter(bool isShooting)
    {
        PlayerAnimatorController.SetBool("Shooting", isShooting);


    }
    [ClientRpc]
    public void RotateSpineResetter()
    {
        movement.RotateSpineReset();

    }

    public virtual void Fire(bool isAutoattack, Vector3 dir)
    {

        // Inherited classes are overriding this method.
    }

    #region Input Methods

    /// <summary>
    /// this method must call from server
    /// </summary>


    public void RotateSpine(float value)
    {
        movement.angle = value;
    }

    /// <summary>
    /// this method must call from server
    /// </summary>
    public void Respawn()
    {
        IsLive = true;
        transform.position = Vector3.zero;
        // OnRevive_DoSomething_Only_On_Other_Client(netIdentity.connectionToClient);
        health.ResetValues();
        RespawnRPC();
    }

    public void OnCurrentUltimateFillRateChanged(float ultimateFillRate)
    {
        if (GameplayPanelUIManager.Instance.joystickCanvas.TryGetComponent(out JoystickCanvasUIController joystickCanvasUIController))
        {

            joystickCanvasUIController.ChangeUltimateFillRate(ultimateFillRate);
        }

    }



    public void EnergyChanged(float energyAmount)
    {
        playerUIHandler.ChangeEnergy(energyAmount);
    }

    public void SetLowerBodyAnimation(Vector3 dir)
    {
        attackDir = CalculateVectors(dir);

    }
    public Vector3 CalculateVectors(Vector3 dir)
    {

        // Debug.Log("moveDir: " + moveDirection);

        //Debug.Log("attackDir: " + dir);
        return dir;

    }

    [TargetRpc]
    public void HandleUltiFillAmount(NetworkConnection target, float ultimateFillRate)
    {

        OnCurrentUltimateFillRateChanged(ultimateFillRate);
    }


    [TargetRpc]
    public void OnUltiActivated(NetworkConnection target)
    {


        GameplayPanelUIManager.Instance.ActivateUltiButton();


    }

    public void ActivateUlti(NetworkConnection target)
    {
        isUltiThrowable = true;
        OnUltiActivated(target);
    }
    [TargetRpc]
    public void DeactivateUltiUI(NetworkConnection target)
    {

        GameplayPanelUIManager.Instance.DeactivateUltiButton();



    }
    //public void DoSomething()
    //{
    //    Vector3 Movedir = new Vector3(movement.moveDirection.x, 0, movement.moveDirection.y).normalized;

    //    //var direction = target.position - transform.position;
    //    //direction.Normalize();

    //    //var offsetVector = Vector3.Cross(attackDir, Movedir);
    //    var offsetVector = Vector3.Cross(Vector3.up, attackDir);
    //    offsetVector.Normalize();
    //    Debug.DrawRay(transform.position, offsetVector, Color.black, 20f);
    //   // var startPosition = transform.position + offsetVector /** localHorizontalOffset*/ + Movedir /** radialOffset*/;
    //    var startPosition = offsetVector ;

    //    Debug.Log("dir: " + startPosition);
    //    var animDir = new Vector3(Mathf.Round(startPosition.normalized.x), 0f, Mathf.Round(startPosition.normalized.z));

    //    if (animDir.x > 0 && Mathf.Abs(animDir.x) > Mathf.Abs(animDir.z))
    //    {
    //        // Debug.Log("pos: " + startPosition);
    //        Debug.Log("right");

    //    }
    //    else if (animDir.x < 0 && Mathf.Abs(animDir.x) > Mathf.Abs(animDir.z))
    //    {
    //        Debug.Log("left");

    //    }
    //    else if (animDir.z > 0 && Mathf.Abs(animDir.x) < Mathf.Abs(animDir.z))
    //    {
    //        Debug.Log("forward");

    //    }
    //    else if (animDir.z < 0 && Mathf.Abs(animDir.x) < Mathf.Abs(animDir.z))
    //    {
    //        Debug.Log("backward");

    //    }

    //    //   Debug.Log("moveDir: " + Movedir);
    //    //  Debug.Log("attackDir: " + attackDir);

    //}


    [ClientRpc]
    public void ShakeEnergyBar()
    {
        playerUIHandler.ShakeEnergyBar();

    }
    Vector2 lastJoystickDirection = -Vector2.one;
    public virtual void Move(Vector2 jostickDirection)
    {
        if (jostickDirection != lastJoystickDirection)
        {
            lastJoystickDirection = jostickDirection;
            var moveDirection = new Vector3(jostickDirection.x, 0f, jostickDirection.y).normalized;
            movement.MovementSpriteHandler(jostickDirection);
            Cmd_MoveToDirection(moveDirection);
        }
        //Debug.Log($"<b> sqrM: {move.sqrMagnitude} </b> move:" + move);
    }
    public void Targeting(Vector2 targetingDirection, bool basicButtonheld = false, bool ultiButtonheld = false)
    {

        attack.Targeting(targetingDirection, basicButtonheld, ultiButtonheld);
    }
    #endregion

    #region RPC Methods
    [Command]
    public virtual void Cmd_MoveToDirection(Vector3 moveDirection)
    {
        movement.MoveToDirection = moveDirection.normalized;
    }
    public override void Death()
    {
        base.Death();

        DropGemsOnDeath();


        StopHealthCoroutine();



        MatchNetworkManager.Instance.Respawn(this);



    }
    public void DropGemsOnDeath()
    {
        CrystalModeNetworkedGameManager gemModeNetworkedGameManager = NetworkedGameManager.Instance as CrystalModeNetworkedGameManager;
        gemModeNetworkedGameManager.OnGemDroppedByThisPlayer(connectionToClient.connectionId);






    }

    public void StopHealthCoroutine()
    {
        PlayerHealth myHealth = health as PlayerHealth;

        myHealth.StopHealthIncreaseCoroutine();
    }

    [ClientRpc]
    public override void DeathRPC()
    {

        base.DeathRPC();

        // TODO: show death panel if localplayer
        if (netIdentity.isLocalPlayer)
        {
            infoPopup = InfoPopup.Show("Loser! You will respawn in 3 second.");

        }
    }
    [ClientRpc]
    public void RespawnRPC()
    {
        IsLive = true;
        // TODO: hide death panel

        if (netIdentity.isLocalPlayer && infoPopup != null)
        {
            infoPopup.Close();
        }
    }

    internal void SetSpineRotator(Transform transform)
    {
        SpineRotator = transform;
        movement.GetCurrentRotateSpine(movement.angle);

    }

    #region Health
    public void HealthRateChanged(float newValue)
    {

        playerUIHandler.ChangeHealthRate(newValue);

    }
    public override void HealthChanged(int health)
    {
        base.HealthChanged(health);
        playerUIHandler.ChangeHealth(health);

    }

    #endregion


    #endregion
}


