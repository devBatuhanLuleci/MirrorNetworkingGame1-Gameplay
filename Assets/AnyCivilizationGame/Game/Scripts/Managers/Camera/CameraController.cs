using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    private float offSetZ;
    private float offSetX;
    [SerializeField]
    private float CameraFollowZSpeed = 1f;
    
    [SerializeField]
    private float CameraFollowXSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        offSetZ = transform.position.z-Target.position.z ;
        offSetX = transform.position.x-Target.position.x ;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =new Vector3(Mathf.Lerp(transform.position.x, Target.position.x + offSetX, Time.deltaTime * CameraFollowXSpeed), transform.position.y,Mathf.Lerp(transform.position.z, Target.position.z+ offSetZ, Time.deltaTime * CameraFollowZSpeed));
    }
}
