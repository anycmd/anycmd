
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class ChangeProcessNetPortCommand: Command, IAnycmdCommand
    {
        public ChangeProcessNetPortCommand(Guid processId, int netPort)
        {
            this.ProcessId = processId;
            this.NetPort = netPort;
        }

        public Guid ProcessId { get; private set; }
        public int NetPort { get; private set; }
    }
}
