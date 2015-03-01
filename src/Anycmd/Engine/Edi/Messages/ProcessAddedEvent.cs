
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class ProcessAddedEvent : DomainEvent
    {
        public ProcessAddedEvent(IAcSession acSession, ProcessBase source)
            : base(acSession, source)
        {
        }
    }
}
