
namespace Anycmd.Engine.Ac.Messages
{
    using InOuts;

    public class AddPrivilegeCommand : AddEntityCommand<IPrivilegeCreateIo>, IAnycmdCommand
    {
        public AddPrivilegeCommand(IAcSession userSession, IPrivilegeCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
