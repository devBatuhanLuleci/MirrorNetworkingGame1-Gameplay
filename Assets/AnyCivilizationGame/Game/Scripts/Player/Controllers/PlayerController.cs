using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public  class PlayerController : NetworkBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;
    private Health health;
    [HideInInspector]
    public PlayerUIHandler playerUIHandler;
    [HideInInspector]
    public Animator PlayerAnimatorController;



    #region Character Projectile Details
    public Transform FirePoint;
    public Transform TargetPoint;
    public LineRenderer temporaryLine;
    public float initialVelocity = 1f;
    #endregion

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<Health>();
        playerUIHandler = GetComponent<PlayerUIHandler>();
        PlayerAnimatorController = GetComponent<Animator>();
    }

    public void SpawnBullet(bool isAutoattack, float angle, Vector3 dir, float speed, float angleNew, float timeNew, float initialVelocity)
    {
       
        // We are spawning Bullet object from object pooler with extra location and rotation parameters.
        var spawnedBullet = ObjectPooler.Instance.Get(attack.Bullet.transform.name, attack.BulletSpawnPoints[0].spawnPoint.position, Quaternion.Euler(0,angle,0)).GetComponent<Bullet>();


        //var spawnedBullet = Instantiate(Bullet, BulletSpawnPoints[0].spawnPoint.position, transform.rotation);

        //threeDProjectile.BulletObj = spawnedBullet;


        var lobbyPlayer = ACGDataManager.Instance.LobbyPlayer;
        //Fire that selected bullet object.
        //var targetPos = transform.forward.normalized * 5;
        //targetPos.y = spawnedBullet.transform.position.y;

        //spawnedBullet.GetComponent<Bullet>().Init(lobbyPlayer.UserName, netId);
        spawnedBullet.Init("Debug User " + netId, netId);

        // spawnedBullet.Throw(new Vector3[] { spawnedBullet.transform.position, targetPos });
        //Debug.Log("stat1:"+ playerController.playerUIHandler.groundDirection.normalized +
        //" stat2:" + playerController.playerUIHandler.v0
        //+ " stat3:" + playerController.playerUIHandler.angle +
        //" stat4:" + playerController.playerUIHandler.timeNew +
        //" stat5:" + playerController.initialVelocity);

      //  Debug.Log("stat1:" + dir + " stat2:" + speed + " stat3:" + angleNew + " stat4:" + timeNew + " stat5:" + initialVelocity);
        spawnedBullet.Throw(dir, speed, angleNew, timeNew, FirePoint, initialVelocity);
        NetworkServer.Spawn(spawnedBullet.gameObject);

    }

    #region Input Methods
    public virtual void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
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

    #region Command Methods
    [Command]
    public virtual void MoveCmd(Vector2 move)
    {
        movement.Move(move);
    }


    #endregion
}
