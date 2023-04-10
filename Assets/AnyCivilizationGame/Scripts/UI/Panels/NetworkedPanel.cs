using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;

public class NetworkedPanel : NetworkBehaviour, IPanel
{
    protected Panel currentPanel;



    public virtual void Close()
    {
        // if panel already close dont make anything
        if (!gameObject.activeSelf) return;

        OnPanelClose();
        gameObject.SetActive(false);
    }
    public virtual void CloseSmoothly()
    {
        //   // if panel already close dont make anything
        //   if (!gameObject.activeSelf) return;

        //   var time = 0f;
        //   while (time > 1)
        //   {
        //       time += Time.fixedDeltaTime;

        //    //   Debug.Log("hmm");

        //       break;
        //   }
        ////   Debug.Log("hello");


        if (TryGetComponent(out CanvasGroup canvasGroup))
        {

            canvasGroup.DOFade(0, 1f).From(1).SetEase(Ease.Linear).OnComplete(()=> {
                OnPanelClose();
                gameObject.SetActive(false);
            });
        }

        


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
    public virtual void ShowSmoothly()
    {
     
      if( TryGetComponent(out CanvasGroup canvasGroup)){

            canvasGroup.DOFade(1, .5f).From(0).SetEase(Ease.Linear);
        }






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
            var item = props[i].GetValue(this) as Panel;
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
