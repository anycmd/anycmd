
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;
    using InOuts;

    public class OntologyOrganizationAddedEvent : DomainEvent
    {
        #region Ctor
        public OntologyOrganizationAddedEvent(OntologyOrganizationBase source, IOntologyOrganizationCreateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }
        #endregion

        public IOntologyOrganizationCreateIo Output { get; private set; }
    }
}
