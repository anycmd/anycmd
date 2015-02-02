
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class BatchRemovedEvent : DomainEvent
    {
        public BatchRemovedEvent(IAcSession acSession, IBatch source)
            : base(acSession, source)
        {
        }
    }
}
