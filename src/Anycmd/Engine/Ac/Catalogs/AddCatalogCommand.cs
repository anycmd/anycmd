
namespace Anycmd.Engine.Ac.Catalogs
{
    using Messages;

    public class AddCatalogCommand : AddEntityCommand<ICatalogCreateIo>, IAnycmdCommand
    {
        public AddCatalogCommand(IAcSession acSession, ICatalogCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
