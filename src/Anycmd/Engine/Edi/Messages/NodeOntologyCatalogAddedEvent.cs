
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    public class NodeOntologyCatalogAddedEvent: DomainEvent
    {
        public NodeOntologyCatalogAddedEvent(IAcSession userSession, NodeOntologyCatalogBase source, INodeOntologyCatalogCreateIo output)
            : base(userSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public INodeOntologyCatalogCreateIo Output { get; private set; }
    }
}
