
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

        internal NodeOntologyCareRemovedEvent(IAcSession acSession, NodeOntologyCareBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
