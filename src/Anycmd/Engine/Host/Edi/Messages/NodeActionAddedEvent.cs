
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class NodeActionAddedEvent : DomainEvent
    {
        public NodeActionAddedEvent(NodeAction source) : base(source) { }
    }
}
