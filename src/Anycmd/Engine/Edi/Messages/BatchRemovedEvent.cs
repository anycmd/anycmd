
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class BatchRemovedEvent : DomainEvent
    {
        public BatchRemovedEvent(IUserSession userSession, IBatch source)
            : base(userSession, source)
        {
        }
    }
}
