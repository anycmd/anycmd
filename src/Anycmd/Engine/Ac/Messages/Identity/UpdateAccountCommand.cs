
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using InOuts;

    public class UpdateAccountCommand : UpdateEntityCommand<IAccountUpdateIo>, IAnycmdCommand
    {
        public UpdateAccountCommand(IUserSession userSession, IAccountUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
