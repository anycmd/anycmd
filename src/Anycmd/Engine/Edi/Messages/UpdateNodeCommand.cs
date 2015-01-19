
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateNodeCommand : UpdateEntityCommand<INodeUpdateIo>, IAnycmdCommand
    {
        public UpdateNodeCommand(IUserSession userSession, INodeUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
