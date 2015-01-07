
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class NodeOntologyCareAddedEvent : DomainEvent
    {
        public NodeOntologyCareAddedEvent(NodeOntologyCareBase source, INodeOntologyCareCreateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public INodeOntologyCareCreateIo Output { get; private set; }
    }
}
