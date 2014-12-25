
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class NodeOntologyCareRemovedEvent : DomainEvent
    {
        public NodeOntologyCareRemovedEvent(NodeOntologyCareBase source) : base(source) { }
    }
}
