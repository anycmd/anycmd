
namespace Anycmd.Engine.Host.Ac.Infra.Messages
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
