
namespace Anycmd.Engine.Ac.Privileges
{
    using Messages;

    public class UpdatePrivilegeCommand : UpdateEntityCommand<IPrivilegeUpdateIo>, IAnycmdCommand
    {
        public UpdatePrivilegeCommand(IAcSession acSession, IPrivilegeUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
