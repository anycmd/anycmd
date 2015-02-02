
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateNodeActionCommand : UpdateEntityCommand<INodeActionUpdateIo>, IAnycmdCommand
    {
        public UpdateNodeActionCommand(IAcSession userSession, INodeActionUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
