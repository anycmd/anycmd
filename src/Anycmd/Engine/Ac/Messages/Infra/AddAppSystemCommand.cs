
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class AddAppSystemCommand : AddEntityCommand<IAppSystemCreateIo>, IAnycmdCommand
    {
        public AddAppSystemCommand(IAppSystemCreateIo input)
            : base(input)
        {

        }
    }
}
