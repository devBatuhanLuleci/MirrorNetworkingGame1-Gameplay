using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FatboyTurret : Throwable,IDamagable
{
    private ObjectController objectController;
    public NetworkAnimator NetworkAnimator;
    public bool shoot = false;
    public Transform TurretRotatePivot;
    public Transform BulletThrowPoint;
    public List<Transform> playerTransforms = new List<Transform>();
    Collider[] playersInsideZone;
    Collider[] playersOutsideZone;
    public float TurretRadius = 4f;
    public bool isLandedSurface = false;
    public float TurretIntervalTimeToShoot = 1f;
    public float TurretSetupTime = .5f;
    public enum TurretShootStatus { Setup, Idle, Shootable, Reloading, Shooting }
    public TurretShootStatus turretShootStatus;
    public bool isDetecting = false;
    public float BulletSpawnIntervalTime = 1f;
    public enum TurretRotation { Idle, Rotating, Focused }
    public TurretRotation turretRotation;
    public Transform CurrentTarget;
    public Coroutine coroutine;
    public float bulletRange = 4f;
    Vector3 dirToTarget;
    float DotValue;
    public float waitForIt = 1.5f;
    public float InitwaitForIt;

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
        InitwaitForIt = waitForIt;
        objectController = GetComponent<ObjectController>();

        turretShootStatus = TurretShootStatus.Setup;
        StartCoroutine(SetupTurret(TurretSetupTime));

    }
    IEnumerator SetupTurret(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        turretShootStatus = TurretShootStatus.Shootable;

        if (isLandedSurface)
        {

            TurretShoot();


        }


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

            if (turretShootStatus == TurretShootStatus.Shootable && turretRotation == TurretRotation.Focused)
            {
                if (CheckAnyEnemy())
                {
                    StartCoroutine(Shoot(waitTime));

                }
                //  Debug.Log("Boom");
            }
            yield return new WaitForEndOfFrame();

        }
    }

    private bool CheckAnyEnemy()
    {
        bool isSomebodyAroundHere = false;


        playersInsideZone = Physics.OverlapSphere(TurretRotatePivot.position, TurretRadius);



        foreach (var obj in playersInsideZone)
        {
            if (obj.TryGetComponent(out PlayerController pc) && pc.netId != OwnerNetId)
            {
                if (pc.TryGetComponent(out Health health))
                {
                    if (health.Value > 0)
                    {
                        isSomebodyAroundHere = true;
                    }
                }

            }
            else if (obj.TryGetComponent(out TurretController tc) && tc.netId != netId)
            {
                if (tc.TryGetComponent(out Health health))
                {
                    if (health.Value > 0)
                    {

                        isSomebodyAroundHere = true;

                    }
                }

            }



        }


        return isSomebodyAroundHere;

    }
    public List<Transform> AllEnemiesInCircle()
    {

        List<Transform> EnemyObjects = new List<Transform>();

      
        playersInsideZone = Physics.OverlapSphere(TurretRotatePivot.position, TurretRadius);



        foreach (var obj in playersInsideZone)
        {
            if (obj.TryGetComponent(out PlayerController pc) && pc.netId != OwnerNetId)
            {
                if (pc.TryGetComponent(out Health health))
                {
                    if (health.Value > 0)
                    {

                        EnemyObjects.Add(obj.transform);
                    }
                }

            }
            else if (obj.TryGetComponent(out TurretController tc) && tc.netId != netId)
            {
                if (tc.TryGetComponent(out Health health))
                {
                    if (health.Value > 0)
                    {

                        EnemyObjects.Add(obj.transform);
                    }
                }

            }

        }
        if (EnemyObjects.Count < 1)
        {
            turretRotation = TurretRotation.Idle;
            waitForIt = InitwaitForIt;
            CurrentTarget = null;

        }

        return EnemyObjects;

    }

    private void FixedUpdate()
    {
        if (!isServer)
        {
            return;
        }



        if (turretShootStatus == TurretShootStatus.Shootable || turretShootStatus == TurretShootStatus.Reloading)
            RotateToTarget(TurretRotatePivot, GetClosestEnemyObject(AllEnemiesInCircle()));


    }
    Transform GetClosestEnemyObject(List<Transform> enemies)
    {
        if (enemies.Count < 1) { return null; }

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
            var rotation = Quaternion.LookRotation(new Vector3((target.position - current.position).x,0, (target.position - current.position).z), Vector3.up);
            current.rotation = Quaternion.Slerp(current.rotation, rotation, Time.deltaTime * 5f);

            //float angle = Quaternion.Angle(current.rotation, rotation);

            dirToTarget = Vector3.Normalize(target.position - current.position);

            DotValue = Vector3.Dot(current.forward, dirToTarget);



            if (target != CurrentTarget)
            {
                waitForIt -= Time.fixedDeltaTime;

                //currentTime = Time.time- starttime;
             
                if (waitForIt <= 0f)
                {
                    turretRotation = TurretRotation.Focused;
                    CurrentTarget = target;
                    waitForIt = InitwaitForIt;
                    //starttime = 0f;
                    //currentTime = 0f;
                }

                if (DotValue == 1f)
                {
                    turretRotation = TurretRotation.Focused;
                    CurrentTarget = target;

                }


            }
            else
            {
                if (DotValue > 0.8f)
                {
                    turretRotation = TurretRotation.Focused;
                }
            }


            //  Debug.Log($" distance : {angle}  ");


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
    public IEnumerator Shoot(float waitTime)
    {
        NetworkAnimator.SetTrigger("Shoot");
        Debug.Log("SHOOT!");
        SpawnBullet(new Vector3[] { TurretRotatePivot.position }, transform.forward, 1, .1f);

        turretShootStatus = TurretShootStatus.Reloading;


        yield return new WaitForSeconds(waitTime);
        turretShootStatus = TurretShootStatus.Shootable;



    }
    public void SpawnBullet(Vector3[] spawnPoint, Vector3 dir, int BulletCount, float BulletIntervalTime)
    {

        StartCoroutine(SpawnIntervalBullet(spawnPoint, dir, BulletCount, BulletIntervalTime));


    }
    public IEnumerator SpawnIntervalBullet(Vector3[] spawnPoint, Vector3 dir, int BulletCount, float MultiplBulletIntervalTime)
    {
        //TODO : burası fire  loop animasyonu entegre edileceği zaman değiecek.






        int currentBulletCount = BulletCount;

        while (currentBulletCount > 0)
        {
            for (int i = 0; i < spawnPoint.Length; i++)
            {

                yield return new WaitForSeconds(MultiplBulletIntervalTime);

                var offsetVector = Vector3.Cross(Vector3.up, dir);
                offsetVector.Normalize();

                string name = "";
                Vector3 pos = Vector3.zero;



                // name = attack.BasicAttackBullet.transform.name;
                name = "FatboyTurret_Bullet";
                pos = BulletThrowPoint.position /*+ offsetVector * spawnPoint[i % spawnPoint.Length].x + transform.up * spawnPoint[i % spawnPoint.Length].y + dir * spawnPoint[i % spawnPoint.Length].z*/;

                var spawnedBullet = ObjectPooler.Instance.Get(name, pos, Quaternion.Euler(0, CalculationManager.GetAngle(dir), 0)).GetComponent<Throwable>();

                spawnedBullet.Init("Debug User " + netId, netId,RootNetId,false);

                spawnedBullet.Throw(dir, bulletRange);
                NetworkServer.Spawn(spawnedBullet.gameObject);
                // spawnedBullet.OnObjectSpawn();


                currentBulletCount--;
                // Debug.Log($"Shoot, currentBulletCount:{currentBulletCount}");

            }

        }
        yield return new WaitForSeconds(BulletSpawnIntervalTime);


    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(TurretRotatePivot.position, TurretRadius);

    }

    public void GetDamage(int damageTaken)
    {
        objectController.TakeDamage(damageTaken);
        Debug.Log($"{damageTaken} Damage taken ");
    }
}
