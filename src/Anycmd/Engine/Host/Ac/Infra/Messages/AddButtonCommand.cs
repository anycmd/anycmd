
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Commands;
    using InOuts;
    using Model;


    public class AddButtonCommand : AddEntityCommand<IButtonCreateIo>, ISysCommand
    {
        public AddButtonCommand(IButtonCreateIo input)
            : base(input)
        {

        }
    }
}
