
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Commands;
    using InOuts;
    using System;

    public class AssignPasswordCommand : Command, IAnycmdCommand
    {
        public AssignPasswordCommand(IUserSession userSession, IPasswordAssignIo input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.UserSession = userSession;
            this.Input = input;
        }

        public IPasswordAssignIo Input { get; private set; }

        public IUserSession UserSession { get; private set; }
    }
}
