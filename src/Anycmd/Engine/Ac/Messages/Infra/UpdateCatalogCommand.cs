
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdateCatalogCommand : UpdateEntityCommand<ICatalogUpdateIo>, IAnycmdCommand
    {
        public UpdateCatalogCommand(IUserSession userSession, ICatalogUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
