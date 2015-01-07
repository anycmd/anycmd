
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class NodeRemovedEvent : DomainEvent
    {
        public NodeRemovedEvent(NodeBase source) : base(source) { }
    }
}
