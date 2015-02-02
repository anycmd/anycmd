
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class NodeElementCareRemovedEvent : DomainEvent
    {
        public NodeElementCareRemovedEvent(IAcSession userSession, NodeElementCareBase source) : base(userSession, source) { }
    }
}
