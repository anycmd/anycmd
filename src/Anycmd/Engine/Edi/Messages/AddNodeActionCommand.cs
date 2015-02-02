
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeActionCommand : AddEntityCommand<INodeActionCreateIo>, IAnycmdCommand
    {
        public AddNodeActionCommand(IAcSession acSession, INodeActionCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
