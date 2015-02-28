﻿
namespace Anycmd.Engine.Ac.Dsd
{
    using Messages;
    using System;

    public class RemoveDsdRoleCommand : RemoveEntityCommand
    {
        public RemoveDsdRoleCommand(IAcSession acSession, Guid dsdRoleId)
            : base(acSession, dsdRoleId)
        {

        }
    }
}
