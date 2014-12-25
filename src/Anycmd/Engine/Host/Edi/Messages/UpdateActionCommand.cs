
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateActionCommand : UpdateEntityCommand<IActionUpdateIo>, ISysCommand
    {
        public UpdateActionCommand(IActionUpdateIo input)
            : base(input)
        {

        }
    }
}
