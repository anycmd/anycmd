
namespace Anycmd.Engine.Edi.Messages
{
    using Events;
    using Info;
    using InOuts;

    public class InfoRuleUpdatedEvent : DomainEvent
    {
        public InfoRuleUpdatedEvent(IUserSession userSession, InfoRuleEntityBase source, IInfoRuleUpdateIo output)
            : base(userSession, source)
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
