using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public Transform Target;
    private float offSetZ;
    private float offSetX;
    [SerializeField]
    private float CameraFollowZSpeed = 1f;
    
    [SerializeField]
    private float CameraFollowXSpeed = 1f;
    // Start is called before the first frame update

    private bool isInitialized = false;
    public void Initialize(Transform player)
    {
        Target = player;
        offSetZ = transform.position.z-Target.position.z ;
        offSetX = transform.position.x-Target.position.x ;
        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized) return; 
        transform.position =new Vector3(Mathf.Lerp(transform.position.x, Target.position.x + offSetX, Time.deltaTime * CameraFollowXSpeed), transform.position.y,Mathf.Lerp(transform.position.z, Target.position.z+ offSetZ, Time.deltaTime * CameraFollowZSpeed));
    }
}
