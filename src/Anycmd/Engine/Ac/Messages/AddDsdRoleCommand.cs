
namespace Anycmd.Engine.Ac.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddDsdRoleCommand : AddEntityCommand<IDsdRoleCreateIo>, ISysCommand
    {
        public AddDsdRoleCommand(IDsdRoleCreateIo input)
            : base(input)
        {

        }
    }
}
