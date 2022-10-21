using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Bullet : MonoBehaviour,IPooledObject
{


    [SerializeField]
    PlayerAttack playerAttack;

    Vector3 BulletEndDist;

    [SerializeField]
    private float speed=0.5f;

    private float BulletTargetOffSetZ;

    private Collider bulletCollider;
    private void Awake()
    {
        bulletCollider = GetComponent<Collider>();
    }
    public void SetPlayer(PlayerAttack playerAttack,float rotAngle, float BulletTargetOffSetZ)
    {
        this.playerAttack = playerAttack;
        this.BulletTargetOffSetZ  = BulletTargetOffSetZ;
        SetRotationOffThisObject(rotAngle);
    }
    public void OnObjectSpawn(PlayerAttack playerAttack, float rotAngle, float BulletTargetOffSetZ)
    {

        SetPlayer( playerAttack,  rotAngle , BulletTargetOffSetZ);
     
    }
 
    public void SetRotationOffThisObject(float rotAngle)
    {
        transform.rotation = Quaternion.Euler(0,rotAngle,0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }





}
