
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public sealed class ButtonAddedEvent : EntityAddedEvent<IButtonCreateIo>
    {
        public ButtonAddedEvent(IAcSession acSession, ButtonBase source, IButtonCreateIo input)
            : base(acSession, source, input)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}