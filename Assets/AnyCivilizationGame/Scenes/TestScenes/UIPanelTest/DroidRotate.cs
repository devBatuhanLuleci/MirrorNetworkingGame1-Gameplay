using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 10f;

    private Touch touch;
    private void OnMouseDrag()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                transform.Rotate(new Vector3(0, -touch.deltaPosition.x, 0) * rotateSpeed * Time.deltaTime);
            }
        }
    }
}
