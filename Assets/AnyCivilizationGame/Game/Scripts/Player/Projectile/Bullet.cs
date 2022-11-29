using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;
using System;

public class Bullet : Throwable, IPooledObject
{
    public int damage = 50;


    public string OwnerName = "";
    public uint OwnerNetId = 0;

    [SerializeField]
    private float speed = 0.5f;


    private void Awake()
    {
        timeOld = speed;
    }

    /// <summary>
    /// This method calls from when the bullet spawn from pool.
    /// </summary>
    /// <param name="rotAngle"></param>
    public void OnObjectSpawn(float rotAngle)
    {
        SetRotationOffThisObject(rotAngle);
    }
    /// <summary>
    /// This function sets the rotation of bullet when it is spawn.
    /// </summary>
    /// <param name="rotAngle"></param>
    public void SetRotationOffThisObject(float rotAngle)
    {
        transform.rotation = Quaternion.Euler(0, rotAngle, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var otherPlayerController) && CanAttack(otherPlayerController))
        {
            otherPlayerController.TakeDamage(damage);
            gameObject.SetActive(false);
            Debug.Log("some one hited by " + OwnerName);
            NetworkServer.Destroy(gameObject);
        }
    }

    private bool CanAttack(PlayerController playerController)
    {
        return OwnerNetId != 0 && playerController.netIdentity.netId != OwnerNetId && netIdentity.isServer;
    }

    public void Init(string ownerName, uint ownerNetId)
    {
        OwnerName = ownerName;
        OwnerNetId = ownerNetId;
    }


    public override void OnArrived()
    {
        NetworkServer.Destroy(gameObject);
    }

}
