
namespace Anycmd.Engine.Ac.UiViews
{

    public class UpdateUiViewCommand : UpdateEntityCommand<IUiViewUpdateIo>, IAnycmdCommand
    {
        public UpdateUiViewCommand(IAcSession acSession, IUiViewUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
