
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class AddCatalogActionCommand : AddEntityCommand<ICatalogActionCreateIo>, IAnycmdCommand
    {
        public AddCatalogActionCommand(IAcSession acSession, ICatalogActionCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
