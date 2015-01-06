
namespace Anycmd.Engine.Ac.Messages
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
