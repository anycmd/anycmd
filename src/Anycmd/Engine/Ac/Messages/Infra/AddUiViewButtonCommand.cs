
namespace Anycmd.Engine.Ac.Messages.Infra
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
