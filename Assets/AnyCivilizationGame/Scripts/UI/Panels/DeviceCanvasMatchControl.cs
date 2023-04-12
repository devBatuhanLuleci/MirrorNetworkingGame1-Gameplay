using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DeviceCanvasMatchControl : MonoBehaviour
{
    [SerializeField] public RectTransform BGPanel;
    private void Awake()
    {
        if (IsDeviceTablet())
        {
            transform.GetComponent<CanvasScaler>().matchWidthOrHeight = .75f;
            RectTransform rt = BGPanel;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 2950f);
        }
        else
        {
            transform.GetComponent<CanvasScaler>().matchWidthOrHeight = .5f;
            RectTransform rt = BGPanel;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 2000f);
        }
    }
    public bool IsDeviceTablet()
    {
        float screenInches = Mathf.Sqrt(Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2)) / Screen.dpi;
        if (screenInches < 7)
        {
            Debug.Log("not tablet");
            return false;
        }
        else
        {
            Debug.Log("tablet");
            return true;
        }

    }
}
