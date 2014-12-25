
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;


    public class AddActionCommand : AddEntityCommand<IActionCreateIo>, ISysCommand
    {
        public AddActionCommand(IActionCreateIo input)
            : base(input)
        {

        }
    }
}
