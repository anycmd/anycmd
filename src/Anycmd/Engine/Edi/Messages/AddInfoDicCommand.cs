
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddInfoDicCommand : AddEntityCommand<IInfoDicCreateIo>, IAnycmdCommand
    {
        public AddInfoDicCommand(IInfoDicCreateIo input)
            : base(input)
        {

        }
    }
}
