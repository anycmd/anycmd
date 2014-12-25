
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class ButtonUpdatedEvent : DomainEvent
    {
        public ButtonUpdatedEvent(ButtonBase source, IButtonUpdateIo input)
            : base(source)
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