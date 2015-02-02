
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class NodeElementCareAddedEvent : DomainEvent
    {
        public NodeElementCareAddedEvent(IAcSession userSession, NodeElementCareBase source, INodeElementCareCreateIo output)
            : base(userSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public INodeElementCareCreateIo Output { get; private set; }
    }
}
