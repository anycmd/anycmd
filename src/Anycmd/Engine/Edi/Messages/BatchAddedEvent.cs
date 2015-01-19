
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class BatchAddedEvent : DomainEvent
    {
        public BatchAddedEvent(IUserSession userSession, IBatch source)
            : base(userSession, source)
        {
        }
    }
}
