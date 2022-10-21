using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzle : MonoBehaviour,IPooledObject
{

    public ParticleSystem particleSystem;
    public void OnObjectSpawn(PlayerAttack playerAttack, float rotAngle , float TargetOffSetZ )
    {
        SetRotationOffThisObject(rotAngle);
    }
    public void SetRotationOffThisObject(float rotAngle)
    {
        transform.rotation = Quaternion.Euler(0, rotAngle, 0);
    }
    void Start()
    {
        var main = particleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;
        
    }
    private void Update()
    {
        if (!particleSystem.IsAlive())
        {
            gameObject.SetActive(false);

        }
    }


}
