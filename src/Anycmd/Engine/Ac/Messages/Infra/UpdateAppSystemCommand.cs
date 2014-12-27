
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateAppSystemCommand : UpdateEntityCommand<IAppSystemUpdateIo>, ISysCommand
    {
        public UpdateAppSystemCommand(IAppSystemUpdateIo input)
            : base(input)
        {

        }
    }
}
