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
    /// <summary>
    /// This method calls from when the bullet spawn from pool.
    /// </summary>
    /// <param name="rotAngle"></param>
    public void OnObjectSpawn( float rotAngle)
    {

        SetRotationOffThisObject(rotAngle);

    }
    /// <summary>
    /// This function sets the rotation of bullet when it is spawn.
    /// </summary>
    /// <param name="rotAngle"></param>
    public void SetRotationOffThisObject(float rotAngle)
    {
        transform.rotation = Quaternion.Euler(0,rotAngle,0);
    }



    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }





}
