
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class ProcessUpdatedEvent : DomainEvent
    {
        public ProcessUpdatedEvent(ProcessBase source)
            : base(source)
        {
        }
    }
}
