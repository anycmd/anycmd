
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Commands;
    using InOuts;
    using System;

    public class ChangePasswordCommand : Command, IAnycmdCommand
    {
        public ChangePasswordCommand(IAcSession userSession, IPasswordChangeIo input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.AcSession = userSession;
            this.Input = input;
        }

        public IPasswordChangeIo Input { get; private set; }

        public IAcSession AcSession { get; private set; }
    }
}
