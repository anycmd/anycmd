
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateNodeCommand : UpdateEntityCommand<INodeUpdateIo>, IAnycmdCommand
    {
        public UpdateNodeCommand(IAcSession userSession, INodeUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
