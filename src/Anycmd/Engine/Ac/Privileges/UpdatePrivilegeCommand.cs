
namespace Anycmd.Engine.Ac.Messages
{
    using InOuts;

    public class UpdatePrivilegeCommand : UpdateEntityCommand<IPrivilegeUpdateIo>, IAnycmdCommand
    {
        public UpdatePrivilegeCommand(IAcSession acSession, IPrivilegeUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
