
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using InOuts;

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
