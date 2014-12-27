
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class ProcessRemovedEvent : DomainEvent
    {
        public ProcessRemovedEvent(ProcessBase source) : base(source) { }
    }
}
