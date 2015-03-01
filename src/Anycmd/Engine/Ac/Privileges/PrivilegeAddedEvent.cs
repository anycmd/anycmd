
namespace Anycmd.Engine.Ac.Privileges
{
    using Events;

    public sealed class PrivilegeAddedEvent : DomainEvent
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
        internal bool IsPrivate { get; set; }
    }
}
