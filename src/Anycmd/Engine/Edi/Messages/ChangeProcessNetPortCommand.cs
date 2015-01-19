
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class ChangeProcessNetPortCommand: Command, IAnycmdCommand
    {
        public ChangeProcessNetPortCommand(IUserSession userSession, Guid processId, int netPort)
        {
            this.UserSession = userSession;
            this.ProcessId = processId;
            this.NetPort = netPort;
        }

        public IUserSession UserSession { get; private set; }

        public Guid ProcessId { get; private set; }
        public int NetPort { get; private set; }
    }
}
