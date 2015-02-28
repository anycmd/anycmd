
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public class UpdateUiViewCommand : UpdateEntityCommand<IUiViewUpdateIo>, IAnycmdCommand
    {
        public UpdateUiViewCommand(IAcSession acSession, IUiViewUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
