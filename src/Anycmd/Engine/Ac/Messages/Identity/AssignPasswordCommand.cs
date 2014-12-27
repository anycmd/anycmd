
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Commands;
    using InOuts;
    using System;

    public class AssignPasswordCommand : Command, ISysCommand
    {
        public AssignPasswordCommand(IPasswordAssignIo input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IPasswordAssignIo Input { get; private set; }
    }
}
