
namespace Anycmd.Engine.Ac.UiViews
{

    /// <summary>
    /// 
    /// </summary>
    public class ButtonAddedEvent : EntityAddedEvent<IButtonCreateIo>
    {
        public ButtonAddedEvent(IAcSession acSession, ButtonBase source, IButtonCreateIo input)
            : base(acSession, source, input)
        {
        }
    }
}