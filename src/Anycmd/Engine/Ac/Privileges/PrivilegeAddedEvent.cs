
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

        internal PrivilegeAddedEvent(IAcSession acSession, PrivilegeBase source, IPrivilegeCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public IPrivilegeCreateIo Output { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}
