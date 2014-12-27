
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;

    public class AddAppSystemCommand : AddEntityCommand<IAppSystemCreateIo>, ISysCommand
    {
        public AddAppSystemCommand(IAppSystemCreateIo input)
            : base(input)
        {

        }
    }
}
