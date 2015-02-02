
namespace Anycmd.Engine.Ac.Messages
{
    using InOuts;

    public class AddPrivilegeCommand : AddEntityCommand<IPrivilegeCreateIo>, IAnycmdCommand
    {
        public AddPrivilegeCommand(IAcSession acSession, IPrivilegeCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
