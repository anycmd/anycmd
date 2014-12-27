
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class NodeElementCareRemovedEvent : DomainEvent
    {
        public NodeElementCareRemovedEvent(NodeElementCareBase source) : base(source) { }
    }
}
