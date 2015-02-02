
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeElementActionCommand : AddEntityCommand<INodeElementActionCreateIo>, IAnycmdCommand
    {
        public AddNodeElementActionCommand(IAcSession userSession, INodeElementActionCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
