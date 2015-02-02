
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeElementActionCommand : AddEntityCommand<INodeElementActionCreateIo>, IAnycmdCommand
    {
        public AddNodeElementActionCommand(IAcSession acSession, INodeElementActionCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
