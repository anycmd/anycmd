
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateGroupCommand : UpdateEntityCommand<IGroupUpdateIo>, ISysCommand
    {
        public UpdateGroupCommand(IGroupUpdateIo input)
            : base(input)
        {

        }
    }
}
