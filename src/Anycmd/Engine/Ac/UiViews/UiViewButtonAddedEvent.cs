
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public sealed class UiViewButtonAddedEvent : EntityAddedEvent<IUiViewButtonCreateIo>
    {
        public UiViewButtonAddedEvent(IAcSession acSession, UiViewButtonBase source, IUiViewButtonCreateIo input)
            : base(acSession, source, input)
        {
        }

        internal UiViewButtonAddedEvent(IAcSession acSession, UiViewButtonBase source, IUiViewButtonCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
