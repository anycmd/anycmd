
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class AddAppSystemCommand : AddEntityCommand<IAppSystemCreateIo>, IAnycmdCommand
    {
        public AddAppSystemCommand(IUserSession userSession, IAppSystemCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
