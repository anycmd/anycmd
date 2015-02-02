
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddOntologyCatalogCommand: AddEntityCommand<IOntologyCatalogCreateIo>, IAnycmdCommand
    {
        public AddOntologyCatalogCommand(IAcSession acSession, IOntologyCatalogCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
