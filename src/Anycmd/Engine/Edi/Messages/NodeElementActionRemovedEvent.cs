
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class NodeElementActionRemovedEvent : DomainEvent
    {
        public NodeElementActionRemovedEvent(IAcSession acSession, NodeElementActionBase source) : base(acSession, source) { }

        internal NodeElementActionRemovedEvent(IAcSession acSession, NodeElementActionBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
