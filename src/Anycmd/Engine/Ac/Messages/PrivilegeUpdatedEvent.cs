
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    public class PrivilegeUpdatedEvent : DomainEvent
    {
        public PrivilegeUpdatedEvent(IUserSession userSession, PrivilegeBase source, IPrivilegeUpdateIo output)
            : base(userSession, source)
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
