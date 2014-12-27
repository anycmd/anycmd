
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class NodeElementActionRemovedEvent : DomainEvent
    {
        public NodeElementActionRemovedEvent(NodeElementActionBase source) : base(source) { }
    }
}
