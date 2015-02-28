
namespace Anycmd.Engine.Ac.Accounts
{
    using Commands;
    using Messages;
    using System;

    public sealed class ChangePasswordCommand : Command, IAnycmdCommand
    {
        public ChangePasswordCommand(IAcSession acSession, IPasswordChangeIo input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.AcSession = acSession;
            this.Input = input;
        }

        public IPasswordChangeIo Input { get; private set; }

        public IAcSession AcSession { get; private set; }
    }
}
