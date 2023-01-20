using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour, IPanel
{
    protected Panel currentPanel;



    public virtual void Close()
    {
        // if panel already close dont make anything
        if (!gameObject.activeSelf) return;

        OnPanelClose();
        gameObject.SetActive(false);
    }
    public virtual void Close(bool destroy)
    {
        Close();
        if (destroy) Destroy(gameObject);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        OnPanelShow();
    }
    public T GetPanel<T>() where T : Panel
    {
        var props = this.GetType().GetFields();
        for (int i = 0; i < props.Length; i++)
        {
            var item = props[i].GetValue(this);
            if (item is T) return item as T;

        }
        return null;
    }
    protected void Show(Panel panel)
    {
        if (!IsChild(panel))
        {
            //  throw new System.Exception($"{panel.name} is not a child.of {this}");
            Debug.LogError($"{panel.name} is not a child.of {this}");
            return;
        }

        currentPanel?.Close();
        panel.Show();
        currentPanel = panel;
    }
    protected bool IsChild(Panel panel)
    {
        var props = this.GetType().GetFields();
        for (int i = 0; i < props.Length; i++)
        {
            var item = props[i].GetValue(this);
            if (item == panel)
            {
                return true;
            }

        }
        return false;
    }
    protected virtual void OnPanelShow() { }
    protected virtual void OnPanelClose() { }


}
