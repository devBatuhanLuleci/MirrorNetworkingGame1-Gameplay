using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AnimateFloat : NetworkBehaviour
{
    public float duration = 2f;
    public AnimationCurve curve;
    public float waitTime = 1f;

    private float time = 0f;
    private bool isAnimating = false;


    public UnityEvent onCountdownTeamInfoPanelFinishedAction;



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

        while (isAnimating)
        {
            // Animation is running, update currentValue
            currentValue = GetCurrentAnimationValue();

            if (hasAnimationStarted && currentValue == 0f)
            {
                OnAnimationFinished();
                hasAnimationStarted = false;
            }

            yield return null;
        }
    }

    private float GetCurrentAnimationValue()
    {
        if (time < duration)
        {
            // Ease.OutCircle from 0 to 1
            float t = curve.Evaluate(time / duration);
            float value = Mathf.Lerp(0f, 1f, t);
            time += Time.deltaTime;
            return value;
        }
        else if (time < duration + waitTime)
        {
            // Wait for waitTime seconds
            time += Time.deltaTime;
            return 1f;
        }
        else if (time < 2f * duration + waitTime)
        {
            // Ease.OutQuad from 1 to 0
            float t = curve.Evaluate((time - duration - waitTime) / duration);
            float value = Mathf.Lerp(1f, 0f, t);
            time += Time.deltaTime;
            hasAnimationStarted = true;
            return value;
        }
        else
        {
            // Ensure the final value is exactly 0
            time = 0f;
            isAnimating = false;
            return 0f;
        }
    }

    public virtual void OnAnimationFinished()
    {
        // Animation is finished, do something here
        Debug.Log("Animation finished");
        onCountdownTeamInfoPanelFinishedAction.Invoke();
    }
}
