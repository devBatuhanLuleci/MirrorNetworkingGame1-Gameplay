using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;

public class Bullet : Throwable, IPooledObject
{



    [SerializeField]
    private float speed = 0.5f;

    public void OnObjectSpawn(float rotAngle)
    {
        SetRotationOffThisObject(rotAngle);
    }

    public void SetRotationOffThisObject(float rotAngle)
    {
        transform.rotation = Quaternion.Euler(0, rotAngle, 0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }





}
