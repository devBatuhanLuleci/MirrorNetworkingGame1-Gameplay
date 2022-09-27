using TMPro;

public class ErrorPanel : PopupPanel
{

    public TextMeshProUGUI errorText;

    public override void Init<T>(T popupValue)
    {
        base.Init(popupValue); 
        if(popupValue is ErrorPanelValue)
        {
            var errorPopupValue = popupValue as ErrorPanelValue;
            errorText.text = errorPopupValue.ErrorText;
        }        
    }
}

public class ErrorPanelValue: IPopupValue
{
    public string ErrorText { get; set; }
}