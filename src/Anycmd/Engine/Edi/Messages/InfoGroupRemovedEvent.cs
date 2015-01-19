
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class InfoGroupRemovedEvent : DomainEvent
    {
        public InfoGroupRemovedEvent(IUserSession userSession, InfoGroupBase source) : base(userSession, source) { }
    }
}
