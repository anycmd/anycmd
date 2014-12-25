
namespace Anycmd.Engine.Host.Ac.Identity.Messages
{
    using Commands;
    using InOuts;
    using System;

    public class ChangePasswordCommand : Command, ISysCommand
    {
        public ChangePasswordCommand(IPasswordChangeIo input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IPasswordChangeIo Input { get; private set; }
    }
}
