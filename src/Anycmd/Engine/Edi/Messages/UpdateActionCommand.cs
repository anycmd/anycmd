
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateActionCommand : UpdateEntityCommand<IActionUpdateIo>, IAnycmdCommand
    {
        public UpdateActionCommand(IActionUpdateIo input)
            : base(input)
        {

        }
    }
}
