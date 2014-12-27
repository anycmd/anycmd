
namespace Anycmd.Engine.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using Events;
    using InOuts;

    public class RoleUpdatedEvent : DomainEvent
    {
        public RoleUpdatedEvent(RoleBase source, IRoleUpdateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IRoleUpdateIo Output { get; private set; }
    }
}