
namespace Anycmd.Engine.Ac.AppSystems
{
    using InOuts;

    public class UpdateAppSystemCommand : UpdateEntityCommand<IAppSystemUpdateIo>, IAnycmdCommand
    {
        public UpdateAppSystemCommand(IAcSession acSession, IAppSystemUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
