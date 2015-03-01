
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

        internal NodeRemovedEvent(IAcSession acSession, NodeBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
