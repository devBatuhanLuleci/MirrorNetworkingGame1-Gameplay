using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour, IPanel
{

    public virtual void Close()
    {
        OnPanelClose();
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        OnPanelShow();
        gameObject.SetActive(true);
    }

    protected virtual void OnPanelShow() { }
    protected virtual void OnPanelClose() { }

}
