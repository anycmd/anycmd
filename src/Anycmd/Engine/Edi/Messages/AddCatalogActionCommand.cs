
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddCatalogActionCommand: AddEntityCommand<ICatalogActionCreateIo>, IAnycmdCommand
    {
        public AddCatalogActionCommand(IAcSession userSession, ICatalogActionCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
