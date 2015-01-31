
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddOntologyCatalogCommand: AddEntityCommand<IOntologyCatalogCreateIo>, IAnycmdCommand
    {
        public AddOntologyCatalogCommand(IUserSession userSession, IOntologyCatalogCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
