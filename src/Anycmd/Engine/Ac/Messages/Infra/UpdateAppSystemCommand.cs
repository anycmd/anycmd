
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdateAppSystemCommand : UpdateEntityCommand<IAppSystemUpdateIo>, IAnycmdCommand
    {
        public UpdateAppSystemCommand(IUserSession userSession, IAppSystemUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
