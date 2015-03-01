
namespace Anycmd.Engine.Ac.Roles
{
    using Events;
    using System;

    public sealed class RoleUpdatedEvent : DomainEvent
    {
        public RoleUpdatedEvent(IAcSession acSession, RoleBase source, IRoleUpdateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            this.Output = output;
        }

        internal RoleUpdatedEvent(IAcSession acSession, RoleBase source, IRoleUpdateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public IRoleUpdateIo Output { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}