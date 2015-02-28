
namespace Anycmd.Engine.Ac.AppSystems
{
    using Messages;

    public sealed class AddAppSystemCommand : AddEntityCommand<IAppSystemCreateIo>, IAnycmdCommand
    {
        public AddAppSystemCommand(IAcSession acSession, IAppSystemCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
