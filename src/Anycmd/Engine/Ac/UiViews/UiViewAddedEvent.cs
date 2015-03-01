
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public sealed class UiViewAddedEvent : EntityAddedEvent<IUiViewCreateIo>
    {
        public UiViewAddedEvent(IAcSession acSession, UiViewBase source, IUiViewCreateIo input)
            : base(acSession, source, input)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}
