
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateCatalogActionCommand : UpdateEntityCommand<ICatalogActionUpdateIo>, IAnycmdCommand
    {
        public UpdateCatalogActionCommand(IAcSession acSession, ICatalogActionUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
