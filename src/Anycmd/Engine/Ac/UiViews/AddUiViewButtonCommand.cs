
namespace Anycmd.Engine.Ac.UiViews
{
    using InOuts;

    public class AddUiViewButtonCommand : AddEntityCommand<IUiViewButtonCreateIo>, IAnycmdCommand
    {
        public AddUiViewButtonCommand(IAcSession acSession, IUiViewButtonCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
