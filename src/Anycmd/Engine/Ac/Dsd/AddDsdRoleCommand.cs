
namespace Anycmd.Engine.Ac.Dsd
{

    public class AddDsdRoleCommand : AddEntityCommand<IDsdRoleCreateIo>, IAnycmdCommand
    {
        public AddDsdRoleCommand(IAcSession acSession, IDsdRoleCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
