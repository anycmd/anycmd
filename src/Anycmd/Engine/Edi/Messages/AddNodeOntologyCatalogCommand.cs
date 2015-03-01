
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class AddNodeOntologyCatalogCommand : AddEntityCommand<INodeOntologyCatalogCreateIo>, IAnycmdCommand
    {
        public AddNodeOntologyCatalogCommand(IAcSession acSession, INodeOntologyCatalogCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
