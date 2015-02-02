﻿
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using Events;

    public class SsdSetRemovedEvent : DomainEvent
    {
        public SsdSetRemovedEvent(IAcSession userSession, SsdSetBase source)
            : base(userSession, source)
        {
        }
    }
}