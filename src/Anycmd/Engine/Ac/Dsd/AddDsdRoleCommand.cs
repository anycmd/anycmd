
namespace Anycmd.Engine.Ac.Dsd
{
    using InOuts;

    public class AddDsdRoleCommand : AddEntityCommand<IDsdRoleCreateIo>, IAnycmdCommand
    {
        public AddDsdRoleCommand(IAcSession acSession, IDsdRoleCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
