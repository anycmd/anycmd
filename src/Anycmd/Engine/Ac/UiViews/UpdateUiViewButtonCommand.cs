
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public sealed class UpdateUiViewButtonCommand : UpdateEntityCommand<IUiViewButtonUpdateIo>, IAnycmdCommand
    {
        public UpdateUiViewButtonCommand(IAcSession acSession, IUiViewButtonUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
