
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ActionRemovedEvent : DomainEvent
    {
        public ActionRemovedEvent(IUserSession userSession, ActionBase source) : base(userSession, source) { }
    }
}
