
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public sealed class NodeAddedEvent : DomainEvent
    {
        public NodeAddedEvent(IAcSession acSession, NodeBase source, INodeCreateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        internal NodeAddedEvent(IAcSession acSession, NodeBase source, INodeCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public INodeCreateIo Output { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}
