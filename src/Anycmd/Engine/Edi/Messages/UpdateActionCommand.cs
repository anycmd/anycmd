
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateActionCommand : UpdateEntityCommand<IActionUpdateIo>, IAnycmdCommand
    {
        public UpdateActionCommand(IAcSession userSession, IActionUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
