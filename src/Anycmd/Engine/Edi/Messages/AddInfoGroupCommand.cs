
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddInfoGroupCommand : AddEntityCommand<IInfoGroupCreateIo>, IAnycmdCommand
    {
        public AddInfoGroupCommand(IInfoGroupCreateIo input)
            : base(input)
        {

        }
    }
}
