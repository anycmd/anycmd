
namespace Anycmd.Engine.Ac.UiViews
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class UiViewUpdatedEvent : DomainEvent
    {
        public UiViewUpdatedEvent(IAcSession acSession, UiViewBase source, IUiViewUpdateIo input)
            : base(acSession, source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        internal UiViewUpdatedEvent(IAcSession acSession, UiViewBase source, IUiViewUpdateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public IUiViewUpdateIo Input { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}
