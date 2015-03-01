
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class AddNodeActionCommand : AddEntityCommand<INodeActionCreateIo>, IAnycmdCommand
    {
        public AddNodeActionCommand(IAcSession acSession, INodeActionCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
