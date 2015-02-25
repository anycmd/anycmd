
namespace Anycmd.Engine.Ac.AppSystems
{
    using InOuts;

    public class AddAppSystemCommand : AddEntityCommand<IAppSystemCreateIo>, IAnycmdCommand
    {
        public AddAppSystemCommand(IAcSession acSession, IAppSystemCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
