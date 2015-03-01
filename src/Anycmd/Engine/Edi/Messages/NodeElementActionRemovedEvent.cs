
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

        internal bool IsPrivate { get; set; }
    }
}
