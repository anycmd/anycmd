
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using Events;

    public class DsdSetRemovedEvent : DomainEvent
    {
        public DsdSetRemovedEvent(DsdSetBase source)
            : base(source)
        {
        }
    }
}