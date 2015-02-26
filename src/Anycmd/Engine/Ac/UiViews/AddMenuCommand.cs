
namespace Anycmd.Engine.Ac.UiViews
{


    public class AddMenuCommand : AddEntityCommand<IMenuCreateIo>, IAnycmdCommand
    {
        public AddMenuCommand(IAcSession acSession, IMenuCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
