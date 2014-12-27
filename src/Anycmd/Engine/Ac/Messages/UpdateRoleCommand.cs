
namespace Anycmd.Engine.Ac.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateRoleCommand : UpdateEntityCommand<IRoleUpdateIo>, ISysCommand
    {
        public UpdateRoleCommand(IRoleUpdateIo input)
            : base(input)
        {

        }
    }
}
