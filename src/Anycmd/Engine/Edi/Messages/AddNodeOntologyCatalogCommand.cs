
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeOntologyCatalogCommand : AddEntityCommand<INodeOntologyCatalogCreateIo>, IAnycmdCommand
    {
        public AddNodeOntologyCatalogCommand(IAcSession acSession, INodeOntologyCatalogCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
