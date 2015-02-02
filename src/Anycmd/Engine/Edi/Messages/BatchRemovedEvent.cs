
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class BatchRemovedEvent : DomainEvent
    {
        public BatchRemovedEvent(IAcSession userSession, IBatch source)
            : base(userSession, source)
        {
        }
    }
}
