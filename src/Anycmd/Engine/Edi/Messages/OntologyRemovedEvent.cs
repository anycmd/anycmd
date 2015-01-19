
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class OntologyRemovedEvent : DomainEvent
    {
        public OntologyRemovedEvent(IUserSession userSession, OntologyBase source) : base(userSession, source) { }
    }
}
