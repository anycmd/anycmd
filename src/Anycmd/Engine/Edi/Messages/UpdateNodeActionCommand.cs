
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public class UpdateNodeActionCommand : UpdateEntityCommand<INodeActionUpdateIo>, IAnycmdCommand
    {
        public UpdateNodeActionCommand(IAcSession acSession, INodeActionUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
