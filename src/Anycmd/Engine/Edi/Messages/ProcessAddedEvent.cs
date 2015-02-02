
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class ProcessAddedEvent : DomainEvent
    {
        public ProcessAddedEvent(IAcSession userSession, ProcessBase source)
            : base(userSession, source)
        {
        }
    }
}
