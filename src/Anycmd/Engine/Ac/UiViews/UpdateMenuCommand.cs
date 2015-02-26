
namespace Anycmd.Engine.Ac.UiViews
{
    using InOuts;

    public class UpdateMenuCommand : UpdateEntityCommand<IMenuUpdateIo>, IAnycmdCommand
    {
        public UpdateMenuCommand(IAcSession acSession, IMenuUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
