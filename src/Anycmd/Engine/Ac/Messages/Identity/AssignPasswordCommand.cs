
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Commands;
    using InOuts;
    using System;

    public class AssignPasswordCommand : Command, IAnycmdCommand
    {
        public AssignPasswordCommand(IUserSession userSession, IPasswordAssignIo input, IUserSession targetSession)
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

        public IPasswordAssignIo Input { get; private set; }

        public IUserSession TargetSession { get; private set; }

        public IUserSession UserSession { get; private set; }
    }
}
