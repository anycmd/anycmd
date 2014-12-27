
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class NodeUpdatedEvent : DomainEvent
    {
        #region Ctor
        public NodeUpdatedEvent(NodeBase source, INodeUpdateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }
        #endregion

        public INodeUpdateIo Output { get; private set; }
    }
}
