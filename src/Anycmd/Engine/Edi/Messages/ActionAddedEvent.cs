
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ActionAddedEvent : DomainEvent
    {
        public ActionAddedEvent(IUserSession userSession, ActionBase source) : base(userSession, source) { }
    }
}
