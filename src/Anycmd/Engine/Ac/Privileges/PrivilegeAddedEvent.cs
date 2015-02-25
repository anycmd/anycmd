
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    public class PrivilegeAddedEvent : DomainEvent
    {
        public PrivilegeAddedEvent(IAcSession acSession, PrivilegeBase source, IPrivilegeCreateIo output)
            : base(acSession, source)
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
