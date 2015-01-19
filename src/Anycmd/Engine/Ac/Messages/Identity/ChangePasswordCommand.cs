
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Commands;
    using InOuts;
    using System;

    public class ChangePasswordCommand : Command, IAnycmdCommand
    {
        public ChangePasswordCommand(IUserSession userSession, IPasswordChangeIo input, IUserSession targetSession)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (targetSession == null)
            {
                throw new ArgumentNullException("targetSession");
            }
            this.UserSession = userSession;
            this.TargetSession = targetSession;
            this.Input = input;
        }

        public IPasswordChangeIo Input { get; private set; }

        public IUserSession TargetSession { get; private set; }

        public IUserSession UserSession { get; private set; }
    }
}
