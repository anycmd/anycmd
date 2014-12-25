
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using Events;

    public class PrivilegeBigramRemovedEvent : DomainEvent
    {
        public PrivilegeBigramRemovedEvent(PrivilegeBigramBase source)
            : base(source)
        {
        }
    }
}
