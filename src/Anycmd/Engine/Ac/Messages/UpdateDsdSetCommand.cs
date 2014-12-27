
namespace Anycmd.Engine.Ac.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateDsdSetCommand : UpdateEntityCommand<IDsdSetUpdateIo>, ISysCommand
    {
        public UpdateDsdSetCommand(IDsdSetUpdateIo input)
            : base(input)
        {

        }
    }
}
