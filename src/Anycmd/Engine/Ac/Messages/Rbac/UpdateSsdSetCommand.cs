
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class UpdateSsdSetCommand: UpdateEntityCommand<ISsdSetUpdateIo>, IAnycmdCommand
    {
        public UpdateSsdSetCommand(IUserSession userSession, ISsdSetUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
