
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddTopicCommand : AddEntityCommand<ITopicCreateIo>, IAnycmdCommand
    {
        public AddTopicCommand(IUserSession userSession, ITopicCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
