
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class UpdateRoleCommand : UpdateEntityCommand<IRoleUpdateIo>, IAnycmdCommand
    {
        public UpdateRoleCommand(IRoleUpdateIo input)
            : base(input)
        {

        }
    }
}
