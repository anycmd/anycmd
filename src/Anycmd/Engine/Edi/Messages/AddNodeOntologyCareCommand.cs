
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeOntologyCareCommand: AddEntityCommand<INodeOntologyCareCreateIo>, IAnycmdCommand
    {
        public AddNodeOntologyCareCommand(IAcSession userSession, INodeOntologyCareCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
