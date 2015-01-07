
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class ProcessAddedEvent : DomainEvent
    {
        public ProcessAddedEvent(ProcessBase source)
            : base(source)
        {
        }
    }
}
