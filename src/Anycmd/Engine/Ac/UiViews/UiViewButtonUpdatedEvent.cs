
namespace Anycmd.Engine.Ac.UiViews
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class UiViewButtonUpdatedEvent : DomainEvent
    {
        public UiViewButtonUpdatedEvent(IAcSession acSession, UiViewButtonBase source, IUiViewButtonUpdateIo input)
            : base(acSession, source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        internal UiViewButtonUpdatedEvent(IAcSession acSession, UiViewButtonBase source, IUiViewButtonUpdateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public IUiViewButtonUpdateIo Input { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}
