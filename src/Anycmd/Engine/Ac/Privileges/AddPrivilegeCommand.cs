
namespace Anycmd.Engine.Ac.Privileges
{

    public class AddPrivilegeCommand : AddEntityCommand<IPrivilegeCreateIo>, IAnycmdCommand
    {
        public AddPrivilegeCommand(IAcSession acSession, IPrivilegeCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
