
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateUiViewCommand : UpdateEntityCommand<IUiViewUpdateIo>, ISysCommand
    {
        public UpdateUiViewCommand(IUiViewUpdateIo input)
            : base(input)
        {

        }
    }
}
