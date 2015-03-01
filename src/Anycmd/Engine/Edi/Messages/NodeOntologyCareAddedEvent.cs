
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public sealed class NodeOntologyCareAddedEvent : DomainEvent
    {
        public NodeOntologyCareAddedEvent(IAcSession acSession, NodeOntologyCareBase source, INodeOntologyCareCreateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        internal NodeOntologyCareAddedEvent(IAcSession acSession, NodeOntologyCareBase source, INodeOntologyCareCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public INodeOntologyCareCreateIo Output { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}
