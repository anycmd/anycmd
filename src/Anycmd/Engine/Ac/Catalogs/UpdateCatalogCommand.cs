
namespace Anycmd.Engine.Ac.Catalogs
{

    public class UpdateCatalogCommand : UpdateEntityCommand<ICatalogUpdateIo>, IAnycmdCommand
    {
        public UpdateCatalogCommand(IAcSession acSession, ICatalogUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
