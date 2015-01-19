
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    public class NodeOntologyOrganizationAddedEvent: DomainEvent
    {
        public NodeOntologyOrganizationAddedEvent(IUserSession userSession, NodeOntologyOrganizationBase source, INodeOntologyOrganizationCreateIo output)
            : base(userSession, source)
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
