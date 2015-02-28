
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public class AddButtonCommand : AddEntityCommand<IButtonCreateIo>, IAnycmdCommand
    {
        public AddButtonCommand(IAcSession acSession, IButtonCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
