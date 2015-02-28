﻿
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public class RemoveProcessCommand : RemoveEntityCommand
    {
        public RemoveProcessCommand(IAcSession acSession, Guid processId)
            : base(acSession, processId)
        {

        }
    }
}
