
namespace Anycmd.Engine.Ac.Ssd
{
    using Messages;

    public sealed class AddSsdRoleCommand : AddEntityCommand<ISsdRoleCreateIo>, IAnycmdCommand
    {
        public AddSsdRoleCommand(IAcSession acSession, ISsdRoleCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
