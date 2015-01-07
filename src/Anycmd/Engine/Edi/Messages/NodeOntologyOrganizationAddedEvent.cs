
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    public class NodeOntologyOrganizationAddedEvent: DomainEvent
    {
        public NodeOntologyOrganizationAddedEvent(NodeOntologyOrganizationBase source, INodeOntologyOrganizationCreateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public INodeOntologyOrganizationCreateIo Output { get; private set; }
    }
}
