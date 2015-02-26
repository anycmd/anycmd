
namespace Anycmd.Engine.Ac.AppSystems
{

    public class AddAppSystemCommand : AddEntityCommand<IAppSystemCreateIo>, IAnycmdCommand
    {
        public AddAppSystemCommand(IAcSession acSession, IAppSystemCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
