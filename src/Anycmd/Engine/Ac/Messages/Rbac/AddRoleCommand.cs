
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class AddRoleCommand : AddEntityCommand<IRoleCreateIo>, IAnycmdCommand
    {
        public AddRoleCommand(IRoleCreateIo input)
            : base(input)
        {

        }
    }
}
