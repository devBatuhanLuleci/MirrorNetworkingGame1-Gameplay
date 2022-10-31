using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicIndicator : MonoBehaviour
{
    private LineRenderer AttackBasicIndicator;
    public LayerMask LayerMask;
    private float TrailDistance;
    private void Awake()
    {
        AttackBasicIndicator = GetComponent<LineRenderer>();
    

    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.GetComponentInParent<PlayerAttack>().transform.position + new Vector3(0, .5f, 0), (transform.GetComponentInParent<PlayerAttack>().lookPos.normalized )* TrailDistance, Color.green);

    }

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

       // if (AttackBasicIndicator.positionCount > 1)
          //  target.position = AttackBasicIndicator.GetPosition(1);
            target.position = player.transform.position + ((lookPos.normalized) * trailDistance);



    }


    public void ResetProjector()
    {

        AttackBasicIndicator.SetPosition(0, Vector3.zero);
        AttackBasicIndicator.SetPosition(1, Vector3.zero);

    }

   


}
