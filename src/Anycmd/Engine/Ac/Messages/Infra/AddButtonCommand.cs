
namespace Anycmd.Engine.Ac.Messages.Infra
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
