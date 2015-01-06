
namespace Anycmd.Engine.Ac.Messages
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
