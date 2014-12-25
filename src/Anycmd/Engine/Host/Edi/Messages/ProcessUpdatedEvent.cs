
namespace Anycmd.Engine.Host.Edi.Messages
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
