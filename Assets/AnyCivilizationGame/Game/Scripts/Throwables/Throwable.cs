using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PredictedProjectileExample.ThreeDProjectile;

public class Throwable : MonoBehaviour
{

    private Coroutine throwingCoroutine;
    float movemenTime = 0;

    public void Throw(Vector3[] path, float time = 2f)
    {
        throwingCoroutine = StartCoroutine(Coroutine_Movement(path, time));
    }

    private void Update()
    {
        if (movemenTime > 0)
        {
            movemenTime += Time.deltaTime;
        }
    }
    public virtual void OnArrived()
    {

    }

    IEnumerator Coroutine_Movement(Vector3[] path, float time)
    {
        movemenTime += Time.deltaTime;
        var flow = movemenTime / time;
        var index = 0; // current positionIndex
        var lastIndex = path.Length - 1; // last index of positions
        var currentPosition = path[index];


        while (index <= lastIndex)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentPosition, flow);
            if (Vector3.Distance(transform.position, currentPosition) <= 0.01f)
            {
                index++;
                currentPosition = path[index];
            }
            yield return null;
        }
        movemenTime = 0;
        OnArrived();
    }

    private void OnDestroy()
    {
        if (throwingCoroutine != null)
            StopCoroutine(throwingCoroutine);
    }
}
