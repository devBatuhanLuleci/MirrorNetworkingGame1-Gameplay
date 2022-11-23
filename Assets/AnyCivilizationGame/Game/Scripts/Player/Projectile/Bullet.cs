using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;

public class Bullet : Throwable, IPooledObject
{
    public int damage = 50;


    [SyncVar]
    public string OwnerName = "";
    [SyncVar]
    public uint OwnerNetId = 0;

    [SerializeField]
    private float speed = 0.5f;

    private void Awake()
    {
        time = speed;
    }


    public void OnObjectSpawn(float rotAngle)
    {
        SetRotationOffThisObject(rotAngle);
    }

    public void SetRotationOffThisObject(float rotAngle)
    {
        transform.rotation = Quaternion.Euler(0, rotAngle, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var enemy) && netIdentity.isServer)
        {
            enemy.TakeDamage(damage);
            gameObject.SetActive(false);
            Debug.Log("some one hited by " + OwnerName);
            NetworkServer.Destroy(gameObject);
        }
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
