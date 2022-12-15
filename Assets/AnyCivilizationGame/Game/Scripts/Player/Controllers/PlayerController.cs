using DG.Tweening;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerController : NetworkBehaviour
{

    #region     Private Fields
    private PlayerMovement movement;
    [HideInInspector]
    public PlayerAttack attack;
    private Health health;

    [HideInInspector]
    public Energy energy;
    [SerializeField]
    public bool IsLive { get; private set; } = true;

    private InfoPopup infoPopup;
    #endregion

    public Transform SpineRotator;
    [HideInInspector]
    public PlayerUIHandler playerUIHandler;
    [HideInInspector]
    public Animator PlayerAnimatorController;

    public List<BulletSpawnPoints> BulletSpawnPoints;

    #region Character Projectile Details

    public Transform TargetPoint;


    public int BulletCount = 1;
    public float BulletIntervalTime = .2f;

    public float StartFireAnimationWaitTime = .2f;
    public float FinishFireAnimationWaitTime = .2f;

    #endregion



    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<Health>();
        energy = GetComponent<Energy>();
        playerUIHandler = GetComponent<PlayerUIHandler>();

    }
    private void Start()
    {
        foreach (BulletSpawnPoints spawnPoint in BulletSpawnPoints)
        {
            spawnPoint.BulletInitPos = spawnPoint.spawnPoint;
        }

    }




    public void SpawnBullet(Vector3[] spawnPoint, Vector3 dir, int BulletCount, float BulletIntervalTime)
    {
        var lobbyPlayer = ACGDataManager.Instance.LobbyPlayer;

        if (energy.CastEnergy())
            StartCoroutine(SpawnIntervalBullet(spawnPoint, dir, BulletCount, BulletIntervalTime));


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
    public IEnumerator SpawnIntervalBullet(Vector3[] spawnPoint, Vector3 dir, int BulletCount, float BulletIntervalTime)
    {
        //TODO : burası fire  loop animasyonu entegre edileceği zaman değiecek.


        yield return new WaitForSeconds(StartFireAnimationWaitTime);
        int currentBulletCount = BulletCount;

        while (currentBulletCount > 0)
        {
            for (int i = 0; i < spawnPoint.Length; i++)
            {
                SetShootingParameter(true);
                yield return new WaitForSeconds(BulletIntervalTime);

                var offsetVector = Vector3.Cross(Vector3.up, dir);
                offsetVector.Normalize();
                var spawnedBullet = ObjectPooler.Instance.Get(attack.Bullet.transform.name, transform.position + offsetVector * spawnPoint[i % spawnPoint.Length].x + transform.up * spawnPoint[i % spawnPoint.Length].y + dir * spawnPoint[i % spawnPoint.Length].z, Quaternion.Euler(0, CalculationManager.GetAngle(dir), 0)).GetComponent<Bullet>();
                spawnedBullet.Init("Debug User " + netId, netId);
                spawnedBullet.Throw(dir);
                NetworkServer.Spawn(spawnedBullet.gameObject);


                currentBulletCount--;
                Debug.Log($"Shoot, currentBulletCount:{currentBulletCount}");

            }

        }
        yield return new WaitForSeconds(FinishFireAnimationWaitTime);
        attack.attackState = PlayerAttack.ShootingState.Idle;

        SetShootingParameter(false);

    }
    [ClientRpc]
    public void SetShootingParameter(bool isShooting)
    {
        PlayerAnimatorController.SetBool("Shooting", isShooting);

    }

    public virtual void Fire(bool isAutoattack, Vector3 dir)
    {
        // Inherited classes are overriding this method.
    }

    #region Input Methods

    /// <summary>
    /// this method must call from server
    /// </summary>
    public virtual void TakeDamage(int damage)
    {
        if (health.TakeDamage(damage))
        {
            // TODO: Player is death 
            // respawn player
            Death();
        }
    }
    /// <summary>
    /// this method must call from server
    /// </summary>
    private void Death()
    {
        IsLive = false;
        MatchNetworkManager.Instance.Respawn(this);
        DeathRPC();
    }

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
        health.ResetValues();
        RespawnRPC();
    }

    public void HealthChanged(int health)
    {
        playerUIHandler.ChangeHealth(health);
    }
    public void EnergyChanged(float energyAmount)
    {
        playerUIHandler.ChangeEnergy(energyAmount);
    }

    [ClientRpc]
    public void ShakeEnergyBar()
    {
        playerUIHandler.ShakeEnergyBar();

    }
    public virtual void Move(Vector2 move)
    {
        movement.MovementSpriteHandler(move);
        if (move.sqrMagnitude > 0f)
        {
            MoveCmd(move);
        }
        //Debug.Log($"<b> sqrM: {move.sqrMagnitude} </b> move:" + move);
    }
    public void Targeting(Vector2 targetingDirection, bool held = false)
    {
        attack.Targeting(targetingDirection, held);
    }
    #endregion

    #region RPC Methods
    [Command]
    public virtual void MoveCmd(Vector2 move)
    {
        movement.Move(move);
    }


    [ClientRpc]
    public void DeathRPC()
    {
        IsLive = false;
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
        SpineRotator.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(movement.angle, movement.angle, movement.angle)), .1f).SetEase(Ease.InOutQuad);

    }


    #endregion
}


