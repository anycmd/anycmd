
namespace Anycmd.Engine.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using Events;
    using InOuts;

    public class PrivilegeUpdatedEvent : DomainEvent
    {
        public PrivilegeUpdatedEvent(PrivilegeBase source, IPrivilegeUpdateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IPrivilegeUpdateIo Output { get; private set; }
    }
}
