
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

        public IRoleUpdateIo Output { get; private set; }
        internal bool IsPrivate { get; set; }
    }
}