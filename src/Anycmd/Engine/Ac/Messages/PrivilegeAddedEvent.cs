
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    public class PrivilegeAddedEvent : DomainEvent
    {
        public PrivilegeAddedEvent(PrivilegeBase source, IPrivilegeCreateIo output)
            : base(source)
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
