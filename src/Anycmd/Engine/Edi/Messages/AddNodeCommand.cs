
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class AddNodeCommand : AddEntityCommand<INodeCreateIo>, IAnycmdCommand
    {
        public AddNodeCommand(IAcSession acSession, INodeCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
