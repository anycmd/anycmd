
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public sealed class NodeElementActionAddedEvent : DomainEvent
    {
        public NodeElementActionAddedEvent(IAcSession acSession, NodeElementActionBase source, INodeElementActionCreateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        internal NodeElementActionAddedEvent(IAcSession acSession, NodeElementActionBase source, INodeElementActionCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public INodeElementActionCreateIo Output { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}
