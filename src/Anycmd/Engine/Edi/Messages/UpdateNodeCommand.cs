
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public class UpdateNodeCommand : UpdateEntityCommand<INodeUpdateIo>, IAnycmdCommand
    {
        public UpdateNodeCommand(IAcSession acSession, INodeUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
