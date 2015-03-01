
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    public sealed class NodeOntologyCatalogAddedEvent : DomainEvent
    {
        public NodeOntologyCatalogAddedEvent(IAcSession acSession, NodeOntologyCatalogBase source, INodeOntologyCatalogCreateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public INodeOntologyCatalogCreateIo Output { get; private set; }
        internal bool IsPrivate { get; set; }
    }
}
