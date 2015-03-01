
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public sealed class AddMenuCommand : AddEntityCommand<IMenuCreateIo>, IAnycmdCommand
    {
        public AddMenuCommand(IAcSession acSession, IMenuCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
