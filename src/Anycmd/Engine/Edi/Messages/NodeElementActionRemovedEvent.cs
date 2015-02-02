
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class NodeElementActionRemovedEvent : DomainEvent
    {
        public NodeElementActionRemovedEvent(IAcSession userSession, NodeElementActionBase source) : base(userSession, source) { }
    }
}
