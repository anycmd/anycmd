
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;


    public class AddUiViewCommand : AddEntityCommand<IUiViewCreateIo>, ISysCommand
    {
        public AddUiViewCommand(IUiViewCreateIo input)
            : base(input)
        {

        }
    }
}
