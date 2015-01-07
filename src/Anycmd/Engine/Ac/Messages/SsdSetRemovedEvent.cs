
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;

    public class SsdSetRemovedEvent : DomainEvent
    {
        public SsdSetRemovedEvent(SsdSetBase source)
            : base(source)
        {
        }
    }
}