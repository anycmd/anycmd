﻿
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ActionAddedEvent : DomainEvent
    {
        public ActionAddedEvent(IAcSession userSession, ActionBase source) : base(userSession, source) { }
    }
}
