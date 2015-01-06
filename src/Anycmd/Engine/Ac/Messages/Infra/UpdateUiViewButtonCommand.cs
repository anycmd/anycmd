
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class UpdateUiViewButtonCommand : UpdateEntityCommand<IUiViewButtonUpdateIo>, IAnycmdCommand
    {
        public UpdateUiViewButtonCommand(IUiViewButtonUpdateIo input)
            : base(input)
        {

        }
    }
}
