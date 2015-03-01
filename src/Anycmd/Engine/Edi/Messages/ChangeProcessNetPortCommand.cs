
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Engine.Messages;
    using System;

    public sealed class ChangeProcessNetPortCommand : Command, IAnycmdCommand
    {
        public ChangeProcessNetPortCommand(IAcSession acSession, Guid processId, int netPort)
        {
            this.AcSession = acSession;
            this.ProcessId = processId;
            this.NetPort = netPort;
        }

        public IAcSession AcSession { get; private set; }

        public Guid ProcessId { get; private set; }
        public int NetPort { get; private set; }
    }
}
