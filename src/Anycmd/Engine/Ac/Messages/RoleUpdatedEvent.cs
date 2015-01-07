
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;
    using InOuts;
    using System;

    public class RoleUpdatedEvent : DomainEvent
    {
        public RoleUpdatedEvent(RoleBase source, IRoleUpdateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IRoleUpdateIo Output { get; private set; }
    }
}