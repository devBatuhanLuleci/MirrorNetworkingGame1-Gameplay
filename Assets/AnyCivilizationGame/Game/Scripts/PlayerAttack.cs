﻿using DG.Tweening;
using PredictedProjectileExample;
using SimpleInputNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.StatusIndicators.Components;

public class PlayerAttack : MonoBehaviour
{

    private Joystick attackJoystick;
    [Space]
    #region States
    public AttackJoystickState attackJoystickState;
    public enum AttackJoystickState { Up, Idle, Holding }

    public ShootingState attackState;
    public enum ShootingState { Idle, Aiming, Reloading, Shooting }

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


    private Transform player;


    private RaycastHit hit;


    public List<BulletSpawnPoint> BulletSpawnPoints;


    public GameObject Bullet;

    private ThreeDProjectile threeDProjectile;






    private void Awake()
    {
        InitilizeVariables();


    }
    private void Start()
    {

        UpdateSelection();
    }
    private void Update()
    {
        SetAttackJoystickState();
        SetLookPosition();
        RotateIndicator();
        SetBulletSpawnPointPosition();


    }

   /// <summary>
   /// We initilize some variables in the begining.
   /// </summary>
    public void InitilizeVariables()
    {
        attackJoystick = DemoGameManager.Instance.AttackJoystick;
        player = this.gameObject.transform;
        threeDProjectile = GetComponent<ThreeDProjectile>();
        Splats = GetComponentInChildren<SplatManager>();

        foreach (BulletSpawnPoint spawnPoint in BulletSpawnPoints)
        {
            spawnPoint.BulletInitPos = spawnPoint.spawnPoint.position;
        }


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

    public void SetAttackJoystickState()
    {

        if (attackJoystick.joystickHeld && attackJoystick.Value.sqrMagnitude <= ClampedAttackJoystickOffset)
        {
            if (attackJoystickState != AttackJoystickState.Idle)
            {
                //    Debug.LogError("Idle");
                attackState = ShootingState.Idle;



                CancelAttackProjectile();

                attackJoystickState = AttackJoystickState.Idle;

            }

        }

        else if (attackJoystick.joystickHeld)
        {
            if (attackJoystickState != AttackJoystickState.Holding)
            {

                //  Debug.LogError("Holding");
                attackState = ShootingState.Aiming;
                attackJoystickState = AttackJoystickState.Holding;
                UpdateSelection();

                SelectAttackProjectile();

            }
        }

        else if (!attackJoystick.joystickHeld && attackJoystick.Value.sqrMagnitude == 0)
        {
            if (attackJoystickState == AttackJoystickState.Holding)
            {
                //Shoot here!

                attackState = ShootingState.Shooting;
                CancelAttackProjectile();

                SpawnBullet();
           

            }

            if (attackJoystickState == AttackJoystickState.Idle)
            {
                Debug.LogError("Auto-Attack");
                attackState = ShootingState.Idle;

            }

            attackJoystickState = AttackJoystickState.Up;

        }

    }

    public float CalculateAngle(Transform from, Transform to)
    {
        float angle = Vector3.Angle((to.position - from.position), Vector3.forward);
        float angle2 = Vector3.Angle((to.position - from.position), Vector3.right);

        if (angle2 > 90)
        {
            angle = 360 - angle;
        }

        return angle;
    }

    private void UpdateSelection()
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
        attackLookAtPoint.position = new Vector3(attackJoystick.Value.x + player.position.x, 0f, attackJoystick.Value.y + player.position.z);

        Vector3 targetDir = attackLookAtPoint.transform.position - player.transform.position;
        lookPos = targetDir;
        lookPos.y = 0;

    }
    private void RotateIndicator()
    {

        if (attackJoystickState == AttackJoystickState.Holding)
        {


            if (splatType == SplatType.BasicIndicator)
            {


                BasicIndicator.RotateProjector(player, lookPos, threeDProjectile.targetPoint, hit, Range);
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
    public void SpawnBullet()
    {
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

         // We are spawning Bullet object from object pooler with extra location and rotation parameters.
        GameObject spawnedBullet = ObjectPooler.Instance.SpawnFromPool(Bullet.transform.name, BulletSpawnPoints[0].spawnPoint.position, transform.rotation, this, CalculateAngle(player, attackLookAtPoint),0);
        threeDProjectile.BulletObj = spawnedBullet;
        //Fire that selected bullet object.
        threeDProjectile.ThrowThisObject();
    }

    /// <summary>
    /// This function handles multiple bullet position on player.
    /// </summary>
    private void SetBulletSpawnPointPosition()
    {



        if (attackJoystickState == AttackJoystickState.Holding)
        {

            if (splatType == SplatType.BasicIndicator)
            {

                var offsetVector = Vector3.Cross(Vector3.up, lookPos.normalized);
                offsetVector.Normalize();

                foreach (BulletSpawnPoint BulletSpawnPoint in BulletSpawnPoints)
                {




                    BulletSpawnPoint.spawnPoint.eulerAngles = new Vector3(0, CalculateAngle(player, attackLookAtPoint), 0);


                    var BulletPosition = new Vector3(player.transform.position.x + (lookPos.normalized.x * BulletSpawnPoint.BulletInitPos.z) + offsetVector.x * BulletSpawnPoint.BulletInitPos.x,
                                                             BulletSpawnPoint.spawnPoint.position.y,
                                                             player.transform.position.z + (lookPos.normalized.z * BulletSpawnPoint.BulletInitPos.z) + offsetVector.z * BulletSpawnPoint.BulletInitPos.x);






                    BulletSpawnPoint.spawnPoint.position = BulletPosition;


                }



            }
        }
    }

}
/// <summary>
/// This class targets one single spawn point.
/// </summary>
[System.Serializable]
public class BulletSpawnPoint
{
    public string SpawnPointName;
    public Transform spawnPoint;
    [HideInInspector]
    public Vector3 BulletInitPos;

}
