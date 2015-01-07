
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;

    public class DsdSetRemovedEvent : DomainEvent
    {
        public DsdSetRemovedEvent(DsdSetBase source)
            : base(source)
        {
        }
    }
}