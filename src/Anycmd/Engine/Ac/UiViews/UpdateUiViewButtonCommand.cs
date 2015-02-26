
namespace Anycmd.Engine.Ac.UiViews
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
