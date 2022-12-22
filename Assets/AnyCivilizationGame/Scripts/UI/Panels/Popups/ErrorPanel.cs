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

    public static void Show(string msg)
    {
        var err = new ErrorPanelValue(msg);
        PopupManager.Show<ErrorPanel>(err);
    }
}

public class ErrorPanelValue: IPopupValue
{
    public string ErrorText { get; set; }
    public ErrorPanelValue()
    {

    }
    public ErrorPanelValue(string errorText)
    {
        ErrorText = errorText;
    }

    
}