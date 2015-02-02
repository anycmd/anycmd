
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class AddRoleCommand : AddEntityCommand<IRoleCreateIo>, IAnycmdCommand
    {
        public AddRoleCommand(IAcSession acSession, IRoleCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
