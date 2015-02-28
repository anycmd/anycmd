
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public class UpdateMenuCommand : UpdateEntityCommand<IMenuUpdateIo>, IAnycmdCommand
    {
        public UpdateMenuCommand(IAcSession acSession, IMenuUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
