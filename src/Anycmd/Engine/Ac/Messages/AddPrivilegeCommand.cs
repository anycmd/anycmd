
namespace Anycmd.Engine.Ac.Messages
{
    using InOuts;

    public class AddPrivilegeCommand : AddEntityCommand<IPrivilegeCreateIo>, IAnycmdCommand
    {
        public AddPrivilegeCommand(IUserSession userSession, IPrivilegeCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
