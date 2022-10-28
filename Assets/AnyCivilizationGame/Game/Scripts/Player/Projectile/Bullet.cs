using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Bullet : MonoBehaviour,IPooledObject
{


    Vector3 BulletEndDist;

    [SerializeField]
    private float speed=0.5f;

    private Collider bulletCollider;
    private void Awake()
    {
        bulletCollider = GetComponent<Collider>();
    }
   
    public void OnObjectSpawn( float rotAngle)
    {

        SetRotationOffThisObject(rotAngle);

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
