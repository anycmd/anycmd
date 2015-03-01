
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

        internal bool IsPrivate { get; set; }
    }
}
