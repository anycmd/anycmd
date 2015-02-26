
namespace Anycmd.Engine.Ac.AppSystems
{

    public class UpdateAppSystemCommand : UpdateEntityCommand<IAppSystemUpdateIo>, IAnycmdCommand
    {
        public UpdateAppSystemCommand(IAcSession acSession, IAppSystemUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
