
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddElementCommand : AddEntityCommand<IElementCreateIo>, IAnycmdCommand
    {
        public AddElementCommand(IElementCreateIo input)
            : base(input)
        {

        }
    }
}
