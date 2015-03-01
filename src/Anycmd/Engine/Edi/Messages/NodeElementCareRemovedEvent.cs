
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class NodeElementCareRemovedEvent : DomainEvent
    {
        public NodeElementCareRemovedEvent(IAcSession acSession, NodeElementCareBase source) : base(acSession, source) { }

        internal NodeElementCareRemovedEvent(IAcSession acSession, NodeElementCareBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
