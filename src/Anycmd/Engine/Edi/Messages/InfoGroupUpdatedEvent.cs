
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class InfoGroupUpdatedEvent : DomainEvent
    {
        public InfoGroupUpdatedEvent(IUserSession userSession, InfoGroupBase source)
            : base(userSession, source)
        {
        }
    }
}
