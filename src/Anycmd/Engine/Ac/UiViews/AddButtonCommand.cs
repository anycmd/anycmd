
namespace Anycmd.Engine.Ac.UiViews
{
    using InOuts;


    public class AddButtonCommand : AddEntityCommand<IButtonCreateIo>, IAnycmdCommand
    {
        public AddButtonCommand(IAcSession acSession, IButtonCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
