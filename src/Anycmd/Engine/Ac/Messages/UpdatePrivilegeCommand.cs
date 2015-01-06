
namespace Anycmd.Engine.Ac.Messages
{
    using InOuts;

    public class UpdatePrivilegeCommand : UpdateEntityCommand<IPrivilegeUpdateIo>, IAnycmdCommand
    {
        public UpdatePrivilegeCommand(IPrivilegeUpdateIo input)
            : base(input)
        {

        }
    }
}
