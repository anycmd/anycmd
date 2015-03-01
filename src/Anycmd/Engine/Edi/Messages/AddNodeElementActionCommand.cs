
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class AddNodeElementActionCommand : AddEntityCommand<INodeElementActionCreateIo>, IAnycmdCommand
    {
        public AddNodeElementActionCommand(IAcSession acSession, INodeElementActionCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
