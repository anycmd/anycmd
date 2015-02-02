
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class NodeOntologyCareRemovedEvent : DomainEvent
    {
        public NodeOntologyCareRemovedEvent(IAcSession acSession, NodeOntologyCareBase source) : base(acSession, source) { }
    }
}
