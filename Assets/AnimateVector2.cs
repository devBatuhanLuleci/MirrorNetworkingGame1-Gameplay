using Mirror;
using System.Collections;
using UnityEngine;

public class AnimateVector2 : NetworkBehaviour
{
    public float duration = 2f;
    public AnimationCurve curve;
    public float waitTime = 1f;

    private float time = 0f;
    private bool isAnimating = false;

    [SyncVar(hook = nameof(OnCurrentValueUpdated))]
    public Vector2 currentValue;

    public virtual void OnCurrentValueUpdated(Vector2 oldValue, Vector2 newValue)
    {
        Debug.Log(currentValue);
    }

    private bool hasAnimationStarted;


    public virtual void Update()
    {
        if (!isServer) { return; }

    }

    public IEnumerator AnimateCoroutine()
    {

        yield return MoveTo(new Vector2(0, -50f), duration);
   
    }

    private IEnumerator MoveTo(Vector2 targetPosition, float duration)
    {
        Vector2 startPosition = new Vector2(0f, 50f);
        currentValue = startPosition;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            currentValue = Vector2.Lerp(startPosition, targetPosition, t);
            time += Time.deltaTime;
            yield return null;
        }
        isAnimating = false;
        currentValue = targetPosition;
        OnAnimationFinished();
    }
   

    public virtual void OnAnimationFinished()
    {
        // Animation is finished, do something here
        Debug.Log("çalışıyor");
    }
}
