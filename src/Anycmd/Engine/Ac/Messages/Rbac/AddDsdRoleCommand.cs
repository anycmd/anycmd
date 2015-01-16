
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class AddDsdRoleCommand : AddEntityCommand<IDsdRoleCreateIo>, IAnycmdCommand
    {
        public AddDsdRoleCommand(IDsdRoleCreateIo input)
            : base(input)
        {

        }
    }
}
