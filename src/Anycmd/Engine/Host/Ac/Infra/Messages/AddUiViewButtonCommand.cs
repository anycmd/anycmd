
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddUiViewButtonCommand : AddEntityCommand<IUiViewButtonCreateIo>, ISysCommand
    {
        public AddUiViewButtonCommand(IUiViewButtonCreateIo input)
            : base(input)
        {

        }
    }
}
