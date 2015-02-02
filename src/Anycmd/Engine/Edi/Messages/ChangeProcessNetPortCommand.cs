﻿
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class ChangeProcessNetPortCommand: Command, IAnycmdCommand
    {
        public ChangeProcessNetPortCommand(IAcSession userSession, Guid processId, int netPort)
        {
            this.AcSession = userSession;
            this.ProcessId = processId;
            this.NetPort = netPort;
        }

        public IAcSession AcSession { get; private set; }

        public Guid ProcessId { get; private set; }
        public int NetPort { get; private set; }
    }
}
