
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdateAppSystemCommand : UpdateEntityCommand<IAppSystemUpdateIo>, IAnycmdCommand
    {
        public UpdateAppSystemCommand(IAcSession userSession, IAppSystemUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
