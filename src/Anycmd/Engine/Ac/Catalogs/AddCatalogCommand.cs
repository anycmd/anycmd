
namespace Anycmd.Engine.Ac.Catalogs
{


    public class AddCatalogCommand : AddEntityCommand<ICatalogCreateIo>, IAnycmdCommand
    {
        public AddCatalogCommand(IAcSession acSession, ICatalogCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
