
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class UpdateUiViewButtonCommand : UpdateEntityCommand<IUiViewButtonUpdateIo>, IAnycmdCommand
    {
        public UpdateUiViewButtonCommand(IAcSession acSession, IUiViewButtonUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
