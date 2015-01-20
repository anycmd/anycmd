
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Commands;
    using InOuts;
    using System;

    public class ChangePasswordCommand : Command, IAnycmdCommand
    {
        public ChangePasswordCommand(IUserSession userSession, IPasswordChangeIo input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.UserSession = userSession;
            this.Input = input;
        }

        public IPasswordChangeIo Input { get; private set; }

        public IUserSession UserSession { get; private set; }
    }
}
