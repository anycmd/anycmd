﻿
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class NodeAddedEvent : DomainEvent
    {
        public NodeAddedEvent(IAcSession userSession, NodeBase source, INodeCreateIo output)
            : base(userSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public INodeCreateIo Output { get; private set; }
    }
}
