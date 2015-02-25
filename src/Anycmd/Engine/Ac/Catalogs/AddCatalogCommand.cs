
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;


    public class AddCatalogCommand : AddEntityCommand<ICatalogCreateIo>, IAnycmdCommand
    {
        public AddCatalogCommand(IAcSession acSession, ICatalogCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
