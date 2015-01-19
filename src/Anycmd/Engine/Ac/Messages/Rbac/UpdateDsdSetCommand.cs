
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class UpdateDsdSetCommand : UpdateEntityCommand<IDsdSetUpdateIo>, IAnycmdCommand
    {
        public UpdateDsdSetCommand(IUserSession userSession, IDsdSetUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
