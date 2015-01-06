
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddInfoDicItemCommand : AddEntityCommand<IInfoDicItemCreateIo>, IAnycmdCommand
    {
        public AddInfoDicItemCommand(IInfoDicItemCreateIo input)
            : base(input)
        {

        }
    }
}
