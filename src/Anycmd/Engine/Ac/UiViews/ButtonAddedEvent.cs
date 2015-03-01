
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public sealed class ButtonAddedEvent : EntityAddedEvent<IButtonCreateIo>
    {
        public ButtonAddedEvent(IAcSession acSession, ButtonBase source, IButtonCreateIo input)
            : base(acSession, source, input)
        {
        }

        internal ButtonAddedEvent(IAcSession acSession, ButtonBase source, IButtonCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}