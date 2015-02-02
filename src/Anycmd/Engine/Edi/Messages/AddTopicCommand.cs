
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddTopicCommand : AddEntityCommand<ITopicCreateIo>, IAnycmdCommand
    {
        public AddTopicCommand(IAcSession acSession, ITopicCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
