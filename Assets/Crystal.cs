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
    private MoveAtoBTransformManager moveAtoBTransformManager;
    private Collider[] colls;
    private Vector3 crystalForceDir;
    public float bounceForwardForceSpeed = 100f;
    public float bounceupForceSpeed = 100f;
    private PlayerController ownerPlayerController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        colls=gameObject.GetComponentsInChildren<Collider>();
        moveAtoBTransformManager = GetComponent<MoveAtoBTransformManager>();
        moveAtoBTransformManager.OnReachedTargetEvent.AddListener(OnReachedTarget);

    }

    private void OnReachedTarget()
    {
        CrystalModeNetworkedGameManager gemModeNetworkedGameManager = NetworkedGameManager.Instance as CrystalModeNetworkedGameManager;
        gemModeNetworkedGameManager.OnGemCollected(ownerPlayerController.connectionToClient.connectionId);
        ownerPlayerController = null;
        NetworkServer.UnSpawn(gameObject);
        ReturnHandler();
    }


    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {

       

        if (NetworkedGameManager.Instance.isGameFinished) { return; }

        if (other.TryGetComponent<PlayerController>(out var otherPlayerController) )
        {
            if (otherPlayerController.IsLive)
            {
            
                OnHitThisPlayer(transform.position,otherPlayerController);
                Debug.Log(" hitted by this man " + OwnerName);

        

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

        ownerPlayerController = otherPlayerController;
  
  

        var startPos = transform;
        var endPos = otherPlayerController.GemCollectPoint.transform;

        moveAtoBTransformManager.InitMoveInfo(startPos,endPos);


     ////   middleGo.transform.position = (startPos.position + endPos.transform.position) / 2f;
     //   var points = new List<Transform> { startPos, /*middleGo.transform,*/ endPos };
     //   crystalMovement.InitInfo(points);

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
        rb.Sleep();
        rb.velocity = Vector3.zero;
       // rb?.AddForce(crystalForceDir * bounceForwardForceSpeed + Vector3.up * bounceupForceSpeed, ForceMode.Force);
    }
   
    public override void InitInfo(Vector3 dir)
    {
        Vector3 groundDir = new Vector3(dir.x, 0, dir.z);
        crystalForceDir = groundDir.normalized;
        base.InitInfo(dir);

    }


}
