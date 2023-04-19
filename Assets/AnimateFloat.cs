using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum AnimationType
{
    ForwardOnly,
    ForwardAndBackward
}

public class AnimateFloat : NetworkBehaviour
{
    public float duration = 2f;
    public AnimationCurve curve;
    public float waitTime = 1f;

    private float time = 0f;
    private bool isAnimating = false;

    public UnityEvent onCountdownFinishedAction;
    public UnityEvent onAnimationFinishedBeforeExtraTimeAction;

    public float initialValue = 0;
    public float endValue = 1;
    public float extraWaitTimeAtTheEnd = 0f;
    public AnimationType animationType = AnimationType.ForwardAndBackward;

    [SyncVar(hook = nameof(OnCurrentValueUpdated))]
    public float currentValue;

    public virtual void OnCurrentValueUpdated(float oldValue, float newValue)
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
        time = 0f;
        isAnimating = true;
        hasAnimationStarted = false;

        float currentStartValue = initialValue;
        float currentEndValue = endValue;

        while (isAnimating)
        {
            // Animation is running, update currentValue
            currentValue = GetCurrentAnimationValue(currentStartValue, currentEndValue);

            yield return null;
        }
        OnAnimationFinishedBeforeExtraTime();
        yield return new WaitForSeconds(extraWaitTimeAtTheEnd);

        OnAnimationFinished();

    }

    private float GetCurrentAnimationValue(float startValue, float endValue)
    {
        if (time < duration)
        {
            // Ease.OutCircle from 0 to 1
            float t = curve.Evaluate(time / duration);
            float value = Mathf.Lerp(startValue, endValue, t);
            time += Time.deltaTime;
            return value;
        }
        else if (time < duration + waitTime)
        {
            // Wait for waitTime seconds
            time += Time.deltaTime;
            return endValue;
        }
        else if (animationType == AnimationType.ForwardAndBackward && time < 2f * duration + waitTime)
        {
            // Ease.OutQuad from 1 to 0
            float t = curve.Evaluate((time - duration - waitTime) / duration);
            float value = Mathf.Lerp(endValue, startValue, t);
            time += Time.deltaTime;
            hasAnimationStarted = true;
            return value;
        }
        else
        {
            // Ensure the final value is exactly startValue
            time = 0f;
            isAnimating = false;

            if (animationType == AnimationType.ForwardAndBackward)
            {
                return startValue;
            }
            else
            {
                return endValue;
            }
        }
    }

    public virtual void OnAnimationFinished()
    {
        Debug.Log("Animation finished");

        // Animation is finished, do something here
        onCountdownFinishedAction.Invoke();
    }
    public virtual void OnAnimationFinishedBeforeExtraTime()
    {
        Debug.Log("AnimationBefore finished");

        // Animation is finished, do something here
        onAnimationFinishedBeforeExtraTimeAction.Invoke();
    }
}
