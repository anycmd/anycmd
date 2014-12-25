
namespace Anycmd.Engine.Host.Edi.Messages
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
