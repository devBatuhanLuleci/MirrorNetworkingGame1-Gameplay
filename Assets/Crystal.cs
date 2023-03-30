using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Crystal : Throwable , INetworkPooledObject
{
    public Action ReturnHandler { get ; set; }
    private Rigidbody rb;
    private CrystalMovement crystalMovement;
    private Collider[] colls;
    private Vector3 crystalForceDir;
    public float bounceForwardForceSpeed = 100f;
    public float bounceupForceSpeed = 100f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        colls=gameObject.GetComponentsInChildren<Collider>();
        crystalMovement=GetComponent<CrystalMovement>();    
    }
    private void OnTriggerEnter(Collider other)
    {



        if (other.TryGetComponent<PlayerController>(out var otherPlayerController) && isServer )
        {
            if (otherPlayerController.IsLive)
            {
            
                OnHitThisPlayer(transform.position,otherPlayerController);
                Debug.Log(" hitted by this man " + OwnerName);

                //GemModeNetworkedGameManager gemModeNetworkedGameManager = NetworkedGameManager.Instance as GemModeNetworkedGameManager;
                //gemModeNetworkedGameManager.OnGemCollected(otherPlayerController.connectionToClient.connectionId);
           
                //NetworkServer.UnSpawn(gameObject);
                //ReturnHandler();

            }

        }
    }
    public void OnHitThisPlayer(Vector3 crystalPos, PlayerController otherPlayerController)
    {
        HandleCollider(false);
        HandleKinematic(true);
        MoveToPlayerPos(crystalPos,otherPlayerController);
    }
    public void MoveToPlayerPos(Vector3 crystalPos, PlayerController otherPlayerController)
    {
        //var startPos = transform.position;
        //var middlePos = (transform.position + otherPlayerController.transform.position) / 2f /*+ (Vector3.up*2)*/;
        //var endPos = otherPlayerController.transform.position;
        //var points = new Vector3[] { startPos, middlePos, endPos };
        GameObject go = new GameObject();
        GameObject middleGo = new GameObject();
        //   go.transform.position = GameObject.Find("StartPoint").transform.position;
        Debug.Log("startPointPos:"+ GameObject.Find("StartPoint").transform.position);
       
        Debug.Log("crystalPos:" + crystalPos);

       go.transform.position = crystalPos;
      
      //  go.transform.position = GameObject.Find("StartPoint").transform.position;
        var startPos = transform;
        var endPos = otherPlayerController.transform;
        middleGo.transform.position = (go.transform.position + endPos.position) / 2f /*+ (Vector3.up*2)*/;
        var points = new Transform[] { go.transform, middleGo.transform, endPos };
 
        crystalMovement.InitInfo(points);

    }
    public void HandleCollider(bool activate)
    {
        foreach (var coll in colls)
        {
            coll.enabled=activate;
        }

    }
    public override void OnObjectSpawn()
    {
        HandleCollider(false);
        HandleKinematic(true);
        Debug.Log("I SPAWNED");
        base.OnObjectSpawn();
    }
    public override void OnArrived()
    {
        
        base.OnArrived();
        HandleCollider(true);
        OnArrived_AddForce();



    }
    void HandleKinematic(bool isKinematic)
    {
        rb.isKinematic=isKinematic;
    }
    private void OnArrived_AddForce()
    {
        HandleKinematic(false);
        rb.velocity = Vector3.zero;
        rb?.AddForce(crystalForceDir * bounceForwardForceSpeed + Vector3.up * bounceupForceSpeed, ForceMode.Force);
    }
   
    public override void InitInfo(Vector3 dir)
    {
        Vector3 groundDir = new Vector3(dir.x, 0, dir.z);
        crystalForceDir = groundDir.normalized;
        base.InitInfo(dir);

    }


}
