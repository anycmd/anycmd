
namespace Anycmd.Engine.Ac.Accounts
{
    using Commands;
    using Messages;
    using System;

    public sealed class AssignPasswordCommand : Command, IAnycmdCommand
    {
        public AssignPasswordCommand(IAcSession acSession, IPasswordAssignIo input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.AcSession = acSession;
            this.Input = input;
        }

        public IPasswordAssignIo Input { get; private set; }

        public IAcSession AcSession { get; private set; }
    }
}
