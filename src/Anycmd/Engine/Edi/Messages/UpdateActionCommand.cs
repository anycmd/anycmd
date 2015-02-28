
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public class UpdateActionCommand : UpdateEntityCommand<IActionUpdateIo>, IAnycmdCommand
    {
        public UpdateActionCommand(IAcSession acSession, IActionUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
