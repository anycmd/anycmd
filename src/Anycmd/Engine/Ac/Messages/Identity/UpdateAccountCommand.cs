
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using InOuts;

    public class UpdateAccountCommand : UpdateEntityCommand<IAccountUpdateIo>, IAnycmdCommand
    {
        public UpdateAccountCommand(IAccountUpdateIo input)
            : base(input)
        {

        }
    }
}
