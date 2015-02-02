
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class NodeActionAddedEvent : DomainEvent
    {
        public NodeActionAddedEvent(IAcSession acSession, NodeAction source) : base(acSession, source) { }
    }
}
