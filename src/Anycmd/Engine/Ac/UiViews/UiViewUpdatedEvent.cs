
namespace Anycmd.Engine.Ac.UiViews
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewUpdatedEvent : DomainEvent
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

        public IUiViewUpdateIo Input { get; private set; }
    }
}
