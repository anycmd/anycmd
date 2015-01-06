
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddTopicCommand : AddEntityCommand<ITopicCreateIo>, IAnycmdCommand
    {
        public AddTopicCommand(ITopicCreateIo input)
            : base(input)
        {

        }
    }
}
