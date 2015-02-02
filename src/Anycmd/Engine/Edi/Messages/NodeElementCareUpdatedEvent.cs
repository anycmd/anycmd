﻿
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class NodeElementCareUpdatedEvent : DomainEvent
    {
        public NodeElementCareUpdatedEvent(IAcSession userSession, NodeElementCareBase source)
            : base(userSession, source)
        {
            this.IsInfoIdItem = source.IsInfoIdItem;
        }

        public bool IsInfoIdItem { get; private set; }
    }
}
