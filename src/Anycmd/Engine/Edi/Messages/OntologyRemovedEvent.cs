
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class OntologyRemovedEvent : DomainEvent
    {
        public OntologyRemovedEvent(IAcSession userSession, OntologyBase source) : base(userSession, source) { }
    }
}
