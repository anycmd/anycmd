
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class ProcessUpdatedEvent : DomainEvent
    {
        public ProcessUpdatedEvent(IAcSession userSession, ProcessBase source)
            : base(userSession, source)
        {
        }
    }
}
