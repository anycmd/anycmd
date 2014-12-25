
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Commands;
    using InOuts;
    using Model;


    public class UpdateUiViewButtonCommand : UpdateEntityCommand<IUiViewButtonUpdateIo>, ISysCommand
    {
        public UpdateUiViewButtonCommand(IUiViewButtonUpdateIo input)
            : base(input)
        {

        }
    }
}
