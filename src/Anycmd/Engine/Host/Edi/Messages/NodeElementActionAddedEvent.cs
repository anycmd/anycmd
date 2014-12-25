
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class NodeElementActionAddedEvent : DomainEvent
    {
        public NodeElementActionAddedEvent(NodeElementActionBase source, INodeElementActionCreateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public INodeElementActionCreateIo Output { get; private set; }
    }
}
