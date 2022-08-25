using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour, IPanel
{

    public virtual void Close()
    {
        // if panel already close dont make anything
        if (!gameObject.activeSelf) return;

        OnPanelClose();
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        OnPanelShow();
    }

    protected virtual void OnPanelShow() { }
    protected virtual void OnPanelClose() { }

 
}
