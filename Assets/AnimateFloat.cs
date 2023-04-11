using Mirror;
using System;
using UnityEngine;

public class AnimateFloat:NetworkBehaviour
{
    public float duration = 2f;
    public AnimationCurve curve;
    public float waitTime = 1f;

    private float time = 0f;
    private bool isAnimating = false;

    [SyncVar(hook = nameof(OnCurrentValueUpdated))]
    public float currentValue;

    public virtual void OnCurrentValueUpdated(float oldValue, float newValue)
    {
        Debug.Log(currentValue);
    }

    private bool hasAnimationStarted;

    
    private void Update()
    {
        if (!isServer) { return; }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            StartAnimation();
        }

        if (!isAnimating)
        {
            return;
        }

        // Animation is running, update currentValue
        currentValue = GetCurrentAnimationValue();

        if (hasAnimationStarted && currentValue == 0f)
        {
            OnAnimationFinished();
            hasAnimationStarted = false;
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
    private void OnAnimationFinished()
    {
        // Animation is finished, do something here
        Debug.Log("Animation finished");
    }

    public void StartAnimation()
    {
        time = 0f;
        isAnimating = true;
        hasAnimationStarted = false;
    }

}