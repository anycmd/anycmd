
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeActionCommand : AddEntityCommand<INodeActionCreateIo>, IAnycmdCommand
    {
        public AddNodeActionCommand(IAcSession userSession, INodeActionCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
