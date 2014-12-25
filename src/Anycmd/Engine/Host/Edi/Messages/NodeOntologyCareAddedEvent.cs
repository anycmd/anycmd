
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
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
