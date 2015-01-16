
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class AddSsdRoleCommand : AddEntityCommand<ISsdRoleCreateIo>, IAnycmdCommand
    {
        public AddSsdRoleCommand(ISsdRoleCreateIo input)
            : base(input)
        {

        }
    }
}
