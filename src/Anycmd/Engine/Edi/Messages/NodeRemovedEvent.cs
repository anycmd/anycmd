
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class NodeRemovedEvent : DomainEvent
    {
        public NodeRemovedEvent(IUserSession userSession, NodeBase source) : base(userSession, source) { }
    }
}
