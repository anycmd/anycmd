
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class NodeElementCareRemovedEvent : DomainEvent
    {
        public NodeElementCareRemovedEvent(IUserSession userSession, NodeElementCareBase source) : base(userSession, source) { }
    }
}
