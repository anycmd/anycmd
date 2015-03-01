
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class UpdateNodeCommand : UpdateEntityCommand<INodeUpdateIo>, IAnycmdCommand
    {
        public UpdateNodeCommand(IAcSession acSession, INodeUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
