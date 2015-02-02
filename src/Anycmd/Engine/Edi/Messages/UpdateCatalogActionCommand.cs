
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateCatalogActionCommand : UpdateEntityCommand<ICatalogActionUpdateIo>, IAnycmdCommand
    {
        public UpdateCatalogActionCommand(IAcSession userSession, ICatalogActionUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
