
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class ProcessUpdatedEvent : DomainEvent
    {
        public ProcessUpdatedEvent(ProcessBase source)
            : base(source)
        {
        }
    }
}
