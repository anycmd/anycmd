
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;


    public class AddActionCommand : AddEntityCommand<IActionCreateIo>, IAnycmdCommand
    {
        public AddActionCommand(IAcSession acSession, IActionCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
