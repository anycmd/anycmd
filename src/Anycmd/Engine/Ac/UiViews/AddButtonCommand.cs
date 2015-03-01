
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public sealed class AddButtonCommand : AddEntityCommand<IButtonCreateIo>, IAnycmdCommand
    {
        public AddButtonCommand(IAcSession acSession, IButtonCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
