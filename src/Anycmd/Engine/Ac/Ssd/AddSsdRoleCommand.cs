
namespace Anycmd.Engine.Ac.Ssd
{

    public class AddSsdRoleCommand : AddEntityCommand<ISsdRoleCreateIo>, IAnycmdCommand
    {
        public AddSsdRoleCommand(IAcSession acSession, ISsdRoleCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
