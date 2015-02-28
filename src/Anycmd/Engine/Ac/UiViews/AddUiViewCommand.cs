
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public class AddUiViewCommand : AddEntityCommand<IUiViewCreateIo>, IAnycmdCommand
    {
        public AddUiViewCommand(IAcSession acSession, IUiViewCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
