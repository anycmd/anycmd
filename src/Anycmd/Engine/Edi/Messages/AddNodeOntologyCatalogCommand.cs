
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeOntologyCatalogCommand : AddEntityCommand<INodeOntologyCatalogCreateIo>, IAnycmdCommand
    {
        public AddNodeOntologyCatalogCommand(IUserSession userSession, INodeOntologyCatalogCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
