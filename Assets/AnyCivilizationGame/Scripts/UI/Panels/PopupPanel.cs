public class PopupPanel : Panel
{
    public virtual void Init<T>(T popupValue) where T : IPopupValue { }
}
