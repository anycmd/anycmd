
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class ProcessRemovedEvent : DomainEvent
    {
        public ProcessRemovedEvent(IAcSession acSession, ProcessBase source) : base(acSession, source) { }
    }
}
