
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using Events;
    using InOuts;

    public class DsdSetUpdatedEvent : DomainEvent
    {
        public DsdSetUpdatedEvent(IAcSession acSession, DsdSetBase source, IDsdSetUpdateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IDsdSetUpdateIo Output { get; private set; }
    }
}