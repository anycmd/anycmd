
namespace Anycmd.Engine.Ac.Privileges
{
    using Messages;

    public sealed class AddPrivilegeCommand : AddEntityCommand<IPrivilegeCreateIo>, IAnycmdCommand
    {
        public AddPrivilegeCommand(IAcSession acSession, IPrivilegeCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
