
namespace Anycmd.Engine.Host.Ac.Identity.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddAccountCommand : AddEntityCommand<IAccountCreateIo>, ISysCommand
    {
        public AddAccountCommand(IAccountCreateIo input)
            : base(input)
        {
        }
    }
}
