
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class BatchUpdatedEvent : DomainEvent
    {
        public BatchUpdatedEvent(IAcSession userSession, IBatch source)
            : base(userSession, source)
        {
        }
    }
}
