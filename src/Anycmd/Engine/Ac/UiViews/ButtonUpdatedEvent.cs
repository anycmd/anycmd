
namespace Anycmd.Engine.Ac.UiViews
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ButtonUpdatedEvent : DomainEvent
    {
        public ButtonUpdatedEvent(IAcSession acSession, ButtonBase source, IButtonUpdateIo input)
            : base(acSession, source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IButtonUpdateIo Input { get; private set; }
        internal bool IsPrivate { get; set; }
    }
}