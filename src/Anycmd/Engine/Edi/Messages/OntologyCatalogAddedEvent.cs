
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    public sealed class OntologyCatalogAddedEvent : DomainEvent
    {
        public OntologyCatalogAddedEvent(IAcSession acSession, OntologyCatalogBase source, IOntologyCatalogCreateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        internal OntologyCatalogAddedEvent(IAcSession acSession, OntologyCatalogBase source, IOntologyCatalogCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public IOntologyCatalogCreateIo Output { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}
