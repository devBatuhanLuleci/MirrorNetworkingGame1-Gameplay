using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Net;

public class DoJumpSample : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform endPoint;
    public float power=1f;
    public int numJumps = 1;
    public float duration = 1f;
    private Vector3 curent;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            curent = endPoint.position;
            // GameObject go= gameObject.CreatePrimitiveObject(transform.position,Color.black,0.4f);
            transform.parent = endPoint;
            transform.DOLocalJump(curent, power, numJumps, duration).OnUpdate(()=> curent=endPoint.position);
          // transform.domove

        }    
    }
}
