
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddElementCommand : AddEntityCommand<IElementCreateIo>, ISysCommand
    {
        public AddElementCommand(IElementCreateIo input)
            : base(input)
        {

        }
    }
}
