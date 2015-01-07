
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    public class OntologyOrganizationAddedEvent : DomainEvent
    {
        public OntologyOrganizationAddedEvent(OntologyOrganizationBase source, IOntologyOrganizationCreateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IOntologyOrganizationCreateIo Output { get; private set; }
    }
}
