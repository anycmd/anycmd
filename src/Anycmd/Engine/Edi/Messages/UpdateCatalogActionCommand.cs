
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class UpdateCatalogActionCommand : UpdateEntityCommand<ICatalogActionUpdateIo>, IAnycmdCommand
    {
        public UpdateCatalogActionCommand(IAcSession acSession, ICatalogActionUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
