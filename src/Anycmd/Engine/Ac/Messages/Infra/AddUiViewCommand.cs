
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddUiViewCommand : AddEntityCommand<IUiViewCreateIo>, IAnycmdCommand
    {
        public AddUiViewCommand(IUiViewCreateIo input)
            : base(input)
        {

        }
    }
}
