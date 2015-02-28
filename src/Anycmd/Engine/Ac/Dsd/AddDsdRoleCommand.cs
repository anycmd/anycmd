
namespace Anycmd.Engine.Ac.Dsd
{
    using Messages;

    public sealed class AddDsdRoleCommand : AddEntityCommand<IDsdRoleCreateIo>, IAnycmdCommand
    {
        public AddDsdRoleCommand(IAcSession acSession, IDsdRoleCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
