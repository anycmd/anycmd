
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class NodeRemovedEvent : DomainEvent
    {
        public NodeRemovedEvent(IAcSession acSession, NodeBase source) : base(acSession, source) { }

        internal bool IsPrivate { get; set; }
    }
}
