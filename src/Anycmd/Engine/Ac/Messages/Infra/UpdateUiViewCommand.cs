
namespace Anycmd.Engine.Ac.Messages.Infra
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
