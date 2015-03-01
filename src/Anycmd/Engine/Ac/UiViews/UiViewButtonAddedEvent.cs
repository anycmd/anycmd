
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public sealed class UiViewButtonAddedEvent : EntityAddedEvent<IUiViewButtonCreateIo>
    {
        public UiViewButtonAddedEvent(IAcSession acSession, UiViewButtonBase source, IUiViewButtonCreateIo input)
            : base(acSession, source, input)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}
