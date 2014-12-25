
namespace Anycmd.Engine.Host.Ac.Infra.Messages
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
