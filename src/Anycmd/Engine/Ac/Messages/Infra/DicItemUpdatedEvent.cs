
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class DicItemUpdatedEvent : DomainEvent
    {
        public DicItemUpdatedEvent(IAcSession acSession, DicItemBase source, IDicItemUpdateIo input)
            : base(acSession, source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IDicItemUpdateIo Input { get; private set; }
    }
}