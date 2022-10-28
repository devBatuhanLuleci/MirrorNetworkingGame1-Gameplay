using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public Transform target;

    public float radialOffset = 2;
    public float localHorizontalOffset = 1;
    private void OnDrawGizmos()
    {
        var direction = target.position - transform.position;
        direction.Normalize();

        var offsetVector = Vector3.Cross(Vector3.up, direction);
        offsetVector.Normalize();
        var startPosition = transform.position + offsetVector * localHorizontalOffset + direction * radialOffset;

        // Sphere
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(target.position, .5f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPosition, .5f);
        // Line
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target.position);


        // startpos
    }
}
