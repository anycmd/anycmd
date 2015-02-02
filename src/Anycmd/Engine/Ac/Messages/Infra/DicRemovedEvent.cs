﻿
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class DicRemovedEvent : DomainEvent
    {
        public DicRemovedEvent(IAcSession userSession, DicBase source)
            : base(userSession, source)
        {
        }
    }
}