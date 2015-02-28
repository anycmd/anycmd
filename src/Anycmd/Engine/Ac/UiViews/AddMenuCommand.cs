
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public class AddMenuCommand : AddEntityCommand<IMenuCreateIo>, IAnycmdCommand
    {
        public AddMenuCommand(IAcSession acSession, IMenuCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
