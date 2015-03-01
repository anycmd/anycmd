
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public sealed class UpdateMenuCommand : UpdateEntityCommand<IMenuUpdateIo>, IAnycmdCommand
    {
        public UpdateMenuCommand(IAcSession acSession, IMenuUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
