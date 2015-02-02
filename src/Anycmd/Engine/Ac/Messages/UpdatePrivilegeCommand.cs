
namespace Anycmd.Engine.Ac.Messages
{
    using InOuts;

    public class UpdatePrivilegeCommand : UpdateEntityCommand<IPrivilegeUpdateIo>, IAnycmdCommand
    {
        public UpdatePrivilegeCommand(IAcSession userSession, IPrivilegeUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
