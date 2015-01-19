
namespace Anycmd.Engine.Ac.Messages
{
    using InOuts;

    public class UpdatePrivilegeCommand : UpdateEntityCommand<IPrivilegeUpdateIo>, IAnycmdCommand
    {
        public UpdatePrivilegeCommand(IUserSession userSession, IPrivilegeUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
