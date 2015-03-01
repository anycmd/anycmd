
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class NodeOntologyCareRemovedEvent : DomainEvent
    {
        public NodeOntologyCareRemovedEvent(IAcSession acSession, NodeOntologyCareBase source) : base(acSession, source) { }

        internal bool IsPrivate { get; set; }
    }
}
