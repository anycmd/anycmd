
namespace Anycmd.Engine.Ac.UiViews
{
    using UiViews;
    using InOuts;

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