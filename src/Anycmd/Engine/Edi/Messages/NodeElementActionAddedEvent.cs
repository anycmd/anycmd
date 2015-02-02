
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class NodeElementActionAddedEvent : DomainEvent
    {
        public NodeElementActionAddedEvent(IAcSession userSession, NodeElementActionBase source, INodeElementActionCreateIo output)
            : base(userSession, source)
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
