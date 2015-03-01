
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

        internal OntologyRemovedEvent(IAcSession acSession, OntologyBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
