
namespace Anycmd.Engine.Ac.UiViews
{
    using InOuts;


    public class AddMenuCommand : AddEntityCommand<IMenuCreateIo>, IAnycmdCommand
    {
        public AddMenuCommand(IAcSession acSession, IMenuCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
