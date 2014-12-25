
namespace Anycmd.Engine.Host.Ac.Identity.Messages
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
