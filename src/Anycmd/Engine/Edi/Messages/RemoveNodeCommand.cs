﻿
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public class RemoveNodeCommand : RemoveEntityCommand
    {
        public RemoveNodeCommand(IAcSession acSession, Guid nodeId)
            : base(acSession, nodeId)
        {

        }
    }
}
