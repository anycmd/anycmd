﻿
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using Events;

    public class DsdSetRemovedEvent : DomainEvent
    {
        public DsdSetRemovedEvent(IAcSession userSession, DsdSetBase source)
            : base(userSession, source)
        {
        }
    }
}