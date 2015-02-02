
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class NodeElementCareRemovedEvent : DomainEvent
    {
        public NodeElementCareRemovedEvent(IAcSession acSession, NodeElementCareBase source) : base(acSession, source) { }
    }
}
