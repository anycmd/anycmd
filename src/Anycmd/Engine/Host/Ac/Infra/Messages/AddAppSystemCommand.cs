
namespace Anycmd.Engine.Host.Ac.Infra.Messages
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
