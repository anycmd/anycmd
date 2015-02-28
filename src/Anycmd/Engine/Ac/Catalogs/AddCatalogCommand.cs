
namespace Anycmd.Engine.Ac.Catalogs
{
    using Messages;

    public sealed class AddCatalogCommand : AddEntityCommand<ICatalogCreateIo>, IAnycmdCommand
    {
        public AddCatalogCommand(IAcSession acSession, ICatalogCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
