
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class NodeActionRemovedEvent : DomainEvent
    {
        public NodeActionRemovedEvent(IAcSession acSession, NodeAction source) : base(acSession, source) { }
    }
}
