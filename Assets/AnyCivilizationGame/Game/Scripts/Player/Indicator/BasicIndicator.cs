using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicIndicator : MonoBehaviour
{
    private LineRenderer AttackBasicIndicator;
    public LayerMask LayerMask;
    private float TrailDistance;
    // PlayerAttack playerAttack;
    private void Awake()
    {
        AttackBasicIndicator = GetComponent<LineRenderer>();
        // playerAttack = transform.GetComponentInParent<PlayerAttack>();

    }
    //private void OnDrawGizmos()
    //{
    //    Debug.DrawRay(playerAttack.transform.position + new Vector3(0, .5f, 0), (playerAttack.lookPos.normalized )* TrailDistance, Color.green);
    //    //Debug.Log((transform.GetComponentInParent<PlayerAttack>().lookPos.normalized) * TrailDistance);
    //}
    // public Vector3 Show()
    // {
    //     // playerAttack.threeDProjectile.targetPoint.position = playerAttack.player.transform.position + ((playerAttack.lookPos.normalized) * playerAttack.Range);
    //     return playerAttack.player.transform.position + ((playerAttack.lookPos.normalized) * playerAttack.Range);
    // }
    public void RotateProjector(Transform player, Vector3 lookPos,Transform target,RaycastHit hit,float trailDistance)
    {

        TrailDistance = trailDistance;
        AttackBasicIndicator.SetPosition(0, player.transform.position + (lookPos.normalized) / 4f );

        if (Physics.Raycast(player.transform.position, (lookPos.normalized), out hit, trailDistance, LayerMask))
        {
            AttackBasicIndicator.SetPosition(1, hit.point);
        }
        else
        {
            AttackBasicIndicator.SetPosition(1, player.transform.position + ((lookPos.normalized) * trailDistance));
        }

    }


    public void ResetProjector()
    {

        AttackBasicIndicator.SetPosition(0, Vector3.zero);
        AttackBasicIndicator.SetPosition(1, Vector3.zero);

    }

   


}
