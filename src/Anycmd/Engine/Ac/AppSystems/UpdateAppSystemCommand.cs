
namespace Anycmd.Engine.Ac.AppSystems
{
    using Messages;

    public class UpdateAppSystemCommand : UpdateEntityCommand<IAppSystemUpdateIo>, IAnycmdCommand
    {
        public UpdateAppSystemCommand(IAcSession acSession, IAppSystemUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
