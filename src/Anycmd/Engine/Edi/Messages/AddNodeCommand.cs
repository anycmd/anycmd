
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeCommand : AddEntityCommand<INodeCreateIo>, IAnycmdCommand
    {
        public AddNodeCommand(IAcSession userSession, INodeCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
