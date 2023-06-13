using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    private static string path = "UI/Popups/";

    public static T Show<T>(IPopupValue popupValue) where T : PopupPanel
    {
        //TODO: If Canvas is not exist, then create one. The If Statement in below.



        var newPath = path + typeof(T);
        var panel = Resources.Load<T>(newPath);
        //var panelTransform = MainPanelUIManager.Instance;
        var panelTransform = FindObjectOfType<PaneUIManager>(true);

        if (panelTransform == null)
        {
            Debug.LogError("UI_Canvas is not found!");
        }
        var popupPanel = Instantiate<T>(panel, panelTransform.transform);
        popupPanel.Init(popupValue);
        return popupPanel;
    }
}
