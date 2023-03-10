using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorTest : MonoBehaviour
{
    public Transform player;
    public Transform target;

    public float radialOffset = 2;
    public float localHorizontalOffset = 1;

    private void OnDrawGizmos()
    {

        var targetPos = new Vector3(target.position.x, 0, target.position.z);
        var playerPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);

        var dirToTarget = targetPos - playerPos;

        var Dot = Vector3.Dot(player.transform.forward, dirToTarget);

        var ex = player.transform.forward * Dot;

      //  var relativePos = player.transform.InverseTransformPoint(playerPos + ex);
        var value = 0f;

        if (Dot > 0)
        {
            value = ex.magnitude;

        }
        else
        {
            value = -ex.magnitude;

        }

        Debug.Log("value:" + value);
        var startPosition = playerPos + ex/*+ offsetVector * localHorizontalOffset + direction * radialOffset*/;


        // Sphere
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(player.transform.position, .2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(target.position, .1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPosition, .1f);
        // Line
        Gizmos.color = Color.red;
        Gizmos.DrawLine(player.transform.position, target.position);


        // startpos
    }

}
