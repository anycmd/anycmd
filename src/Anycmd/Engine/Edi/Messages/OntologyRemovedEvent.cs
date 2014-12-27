
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class OntologyRemovedEvent : DomainEvent
    {
        public OntologyRemovedEvent(OntologyBase source) : base(source) { }
    }
}
