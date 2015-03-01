
namespace Anycmd.Engine.Edi.Messages
{
    using Events;
    using Info;
    using InOuts;

    public sealed class InfoRuleUpdatedEvent : DomainEvent
    {
        public InfoRuleUpdatedEvent(IAcSession acSession, InfoRuleEntityBase source, IInfoRuleUpdateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IInfoRuleUpdateIo Output { get; private set; }
    }
}
