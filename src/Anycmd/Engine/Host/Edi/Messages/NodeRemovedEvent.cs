
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class NodeRemovedEvent : DomainEvent
    {
        public NodeRemovedEvent(NodeBase source) : base(source) { }
    }
}
