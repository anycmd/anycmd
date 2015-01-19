
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class NodeActionRemovedEvent : DomainEvent
    {
        public NodeActionRemovedEvent(IUserSession userSession, NodeAction source) : base(userSession, source) { }
    }
}
