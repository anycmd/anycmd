
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public sealed class NodeElementCareAddedEvent : DomainEvent
    {
        public NodeElementCareAddedEvent(IAcSession acSession, NodeElementCareBase source, INodeElementCareCreateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public INodeElementCareCreateIo Output { get; private set; }
        internal bool IsPrivate { get; set; }
    }
}
