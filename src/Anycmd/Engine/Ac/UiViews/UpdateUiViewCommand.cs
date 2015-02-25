
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdateUiViewCommand : UpdateEntityCommand<IUiViewUpdateIo>, IAnycmdCommand
    {
        public UpdateUiViewCommand(IAcSession acSession, IUiViewUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
