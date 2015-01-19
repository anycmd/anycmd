
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class InfoGroupAddedEvent : DomainEvent
    {
        public InfoGroupAddedEvent(IUserSession userSession, InfoGroupBase source) : base(userSession, source) { }
    }
}
