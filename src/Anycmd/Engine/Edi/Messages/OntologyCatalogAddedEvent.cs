
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    public class OntologyCatalogAddedEvent : DomainEvent
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

        public IOntologyCatalogCreateIo Output { get; private set; }
    }
}
