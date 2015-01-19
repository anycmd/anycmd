
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeOntologyCareCommand: AddEntityCommand<INodeOntologyCareCreateIo>, IAnycmdCommand
    {
        public AddNodeOntologyCareCommand(IUserSession userSession, INodeOntologyCareCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
