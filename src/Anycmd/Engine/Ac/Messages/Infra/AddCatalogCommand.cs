
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddCatalogCommand : AddEntityCommand<ICatalogCreateIo>, IAnycmdCommand
    {
        public AddCatalogCommand(IUserSession userSession, ICatalogCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
