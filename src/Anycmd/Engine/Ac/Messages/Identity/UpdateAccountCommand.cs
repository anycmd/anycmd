
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateAccountCommand : UpdateEntityCommand<IAccountUpdateIo>, ISysCommand
    {
        public UpdateAccountCommand(IAccountUpdateIo input)
            : base(input)
        {

        }
    }
}
