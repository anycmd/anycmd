
namespace Anycmd.Engine.Ac.Privileges
{
    using Events;

    public sealed class PrivilegeUpdatedEvent : DomainEvent
    {
        public PrivilegeUpdatedEvent(IAcSession acSession, PrivilegeBase source, IPrivilegeUpdateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        internal PrivilegeUpdatedEvent(IAcSession acSession, PrivilegeBase source, IPrivilegeUpdateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public IPrivilegeUpdateIo Output { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}
