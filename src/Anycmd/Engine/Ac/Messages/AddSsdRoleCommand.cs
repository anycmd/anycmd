
namespace Anycmd.Engine.Ac.Messages
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
