
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public sealed class UiViewAddedEvent : EntityAddedEvent<IUiViewCreateIo>
    {
        public UiViewAddedEvent(IAcSession acSession, UiViewBase source, IUiViewCreateIo input)
            : base(acSession, source, input)
        {
        }

        internal UiViewAddedEvent(IAcSession acSession, UiViewBase source, IUiViewCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
