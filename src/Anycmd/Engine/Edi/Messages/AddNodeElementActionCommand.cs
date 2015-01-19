
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeElementActionCommand : AddEntityCommand<INodeElementActionCreateIo>, IAnycmdCommand
    {
        public AddNodeElementActionCommand(IUserSession userSession, INodeElementActionCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
