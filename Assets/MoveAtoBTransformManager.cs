
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveAtoBTransformManager : MonoBehaviour
{
    //public Transform target;
    public float duration = 3f;
    public float jumpiness = .3f;
    public UnityEvent OnReachedTargetEvent;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
         
        }
    }

    public void InitMoveInfo(Transform startPos, Transform target)
    {

        StartCoroutine(MoveTo(startPos,target, duration, jumpiness));

    }
    public IEnumerator MoveTo(Transform startPos,Transform target, float duration, float height)
    {
        Vector3 startingPosition = startPos.position;
        Vector3 targetPosition = target.position+Vector3.up*.5f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            transform.position = Vector3.Lerp(startingPosition, targetPosition, t) + Vector3.up * Mathf.Abs(Mathf.Sin(t * Mathf.PI)) * height;

            timeElapsed += Time.deltaTime;


            targetPosition = target.position;
            yield return null;
        }
        OnObjectArrived();
        transform.position = targetPosition;
    }

    public void OnObjectArrived()
    {
        Debug.Log("Arrıved");
        OnReachedTargetEvent.Invoke();
    }

}
