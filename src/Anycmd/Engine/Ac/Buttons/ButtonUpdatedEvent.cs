
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class ButtonUpdatedEvent : DomainEvent
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
    }
}