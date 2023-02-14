using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FatboyTurret : Throwable
{

    public NetworkAnimator NetworkAnimator;
    public bool shoot = false;
    public Transform TurretRotatePivot;
    public List<Transform> playerTransforms = new List<Transform>();
    Collider[] playersInsideZone;
    Collider[] playersOutsideZone;
    public float TurretRadius = 4f;
    public bool isLandedSurface = false;
    public float TurretIntervalTimeToShoot = 1f;
    public float TurretSetupTime = .5f;
    public enum TurretShootStatus { Setup,Shootable, Reloading, Shooting }
    public TurretShootStatus turretShootStatus;
    public bool isDetecting = false;

    public enum TurretRotation { Idle, Rotating, Focused }
    public TurretRotation turretRotation;
    public Transform CurrentTarget;
    public Coroutine coroutine;

    private void Awake()
    {
        if (!isServer)
        {
            return;

        }


    }

    public override void OnArrived()
    {
        //base.OnArrived();
        isLandedSurface = true;
        turretShootStatus = TurretShootStatus.Setup;
        StartCoroutine(SetupTurret(TurretSetupTime));

        if (isLandedSurface)
        {

            TurretShoot();


        }
    }
    IEnumerator SetupTurret(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        turretShootStatus = TurretShootStatus.Shootable;

    }

    public void TurretShoot()
    {
        coroutine = StartCoroutine(ShootInterval(TurretIntervalTimeToShoot));

    }
    IEnumerator ShootInterval(float waitTime)
    {
        // IEnumurtor yerine update'de Time.time ile yap.
        while (true)
        {
            
            if (CurrentTarget != null && turretShootStatus == TurretShootStatus.Shootable)
            {

                Shoot();
                Debug.Log("Boom");
                turretShootStatus = TurretShootStatus.Reloading;
            }
            yield return new WaitForSeconds(waitTime);
            turretShootStatus = TurretShootStatus.Shootable;

        }
    }
    private void Update()
    {
        if (!isServer)
        {
            return;
        }
     


        if (turretShootStatus == TurretShootStatus.Shootable || turretShootStatus == TurretShootStatus.Reloading)
            RotateToTarget(TurretRotatePivot, GetClosestEnemy(GetEnemies()));


    }
    Transform GetClosestEnemy(List<Transform> enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in enemies)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }
    public void RotateToTarget(Transform current, Transform target)
    {
        if (target != null)
        {
            turretRotation = TurretRotation.Rotating;
            //var lookPos = new Vector3(moveDirection.x, 0f, moveDirection.y).normalized;
            var rotation = Quaternion.LookRotation(target.position - current.position, Vector3.up);
            current.rotation = Quaternion.Slerp(current.rotation, rotation, Time.deltaTime * 5f);

            if (current.rotation.eulerAngles.magnitude - rotation.eulerAngles.magnitude <1f)
            {
                turretRotation = TurretRotation.Focused;
                CurrentTarget = target;
               
            }

            Debug.DrawLine(current.position, current.position + current.forward * 5, Color.blue);
            Debug.DrawLine(current.position, current.position + current.up * 2, Color.green);
        }
    }
    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        ActivateFatBoyTurretAnimation();
    }

    public void ActivateFatBoyTurretAnimation()
    {
        if (NetworkAnimator != null)
        {

            Debug.Log($"IsClient : {isClient} , IsServer : {isServer}");
            NetworkAnimator.SetTrigger("Setup");
        }




    }
    public void Shoot()
    {
        turretShootStatus = TurretShootStatus.Shooting;
        NetworkAnimator.SetTrigger("Shoot");




    }
    public List<Transform> GetEnemies()
    {
        List<Transform> playerControllers = new List<Transform>();

        playersInsideZone = Physics.OverlapSphere(TurretRotatePivot.position, TurretRadius);



        //    Physics.CheckSphere(TurretRotatePivot.position,TurretRadius)

        foreach (var obj in playersInsideZone)
        {
            if (obj.TryGetComponent(out PlayerController pc))
            {

                playerControllers.Add(obj.transform);

            }
            else
            {
                if (CurrentTarget != null)
                {
                    CurrentTarget = null;

                }
                turretShootStatus = TurretShootStatus.Shootable;
                turretRotation = TurretRotation.Idle;
            }

        }


        return playerControllers.ToList();




        //foreach (var player in playerControllers)
        //{
        //   // if(player.is)
        //    playerTransforms.Add(player.transform);
        //}

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(TurretRotatePivot.position, TurretRadius);

    }
}
