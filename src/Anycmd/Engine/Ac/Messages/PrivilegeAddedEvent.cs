
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    public class PrivilegeAddedEvent : DomainEvent
    {
        public PrivilegeAddedEvent(IAcSession userSession, PrivilegeBase source, IPrivilegeCreateIo output)
            : base(userSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IPrivilegeCreateIo Output { get; private set; }
    }
}
