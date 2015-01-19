
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateNodeActionCommand : UpdateEntityCommand<INodeActionUpdateIo>, IAnycmdCommand
    {
        public UpdateNodeActionCommand(IUserSession userSession, INodeActionUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
