
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class ProcessAddedEvent : DomainEvent
    {
        public ProcessAddedEvent(ProcessBase source)
            : base(source)
        {
        }
    }
}
