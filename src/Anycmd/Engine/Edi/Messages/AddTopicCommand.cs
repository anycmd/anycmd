
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class AddTopicCommand : AddEntityCommand<ITopicCreateIo>, IAnycmdCommand
    {
        public AddTopicCommand(IAcSession acSession, ITopicCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
