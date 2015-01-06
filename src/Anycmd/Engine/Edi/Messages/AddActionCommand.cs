
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;


    public class AddActionCommand : AddEntityCommand<IActionCreateIo>, IAnycmdCommand
    {
        public AddActionCommand(IActionCreateIo input)
            : base(input)
        {

        }
    }
}
