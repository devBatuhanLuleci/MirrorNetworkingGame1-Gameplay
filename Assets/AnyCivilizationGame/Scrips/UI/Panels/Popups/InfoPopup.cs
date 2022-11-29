using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPopup : PopupPanel
{
    public TextMeshProUGUI infoText;

    public override void Init<T>(T popupValue)
    {
        base.Init(popupValue);
        if (popupValue is InfoPopupValue)
        {
            var errorPopupValue = popupValue as InfoPopupValue;
            infoText.text = errorPopupValue.Info;
        }
    }

    public static InfoPopup Show(string msg)
    {
        var info = new InfoPopupValue(msg);
        return PopupManager.Show<InfoPopup>(info);
    }


}

public class InfoPopupValue : IPopupValue
{
    public string Info { get; set; }
    public InfoPopupValue()
    {

    }
    public InfoPopupValue(string info)
    {
        Info = info;
    }

}