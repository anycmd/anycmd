
namespace Anycmd.Engine.Host.Edi.Messages
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
