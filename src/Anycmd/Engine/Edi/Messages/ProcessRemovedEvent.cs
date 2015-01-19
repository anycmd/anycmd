
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class ProcessRemovedEvent : DomainEvent
    {
        public ProcessRemovedEvent(IUserSession userSession, ProcessBase source) : base(userSession, source) { }
    }
}
