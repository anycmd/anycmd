
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;


    public class AddActionCommand : AddEntityCommand<IActionCreateIo>, IAnycmdCommand
    {
        public AddActionCommand(IAcSession userSession, IActionCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
