
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class OntologyRemovedEvent : DomainEvent
    {
        public OntologyRemovedEvent(IAcSession acSession, OntologyBase source) : base(acSession, source) { }

        internal bool IsPrivate { get; set; }
    }
}
