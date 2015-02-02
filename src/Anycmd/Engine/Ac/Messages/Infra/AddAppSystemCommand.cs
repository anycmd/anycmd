
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class AddAppSystemCommand : AddEntityCommand<IAppSystemCreateIo>, IAnycmdCommand
    {
        public AddAppSystemCommand(IAcSession userSession, IAppSystemCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
