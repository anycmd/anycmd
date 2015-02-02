
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddCatalogActionCommand: AddEntityCommand<ICatalogActionCreateIo>, IAnycmdCommand
    {
        public AddCatalogActionCommand(IAcSession acSession, ICatalogActionCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
