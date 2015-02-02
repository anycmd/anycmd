
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class UpdateDsdSetCommand : UpdateEntityCommand<IDsdSetUpdateIo>, IAnycmdCommand
    {
        public UpdateDsdSetCommand(IAcSession acSession, IDsdSetUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
