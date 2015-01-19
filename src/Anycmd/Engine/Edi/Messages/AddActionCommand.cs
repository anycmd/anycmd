
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;


    public class AddActionCommand : AddEntityCommand<IActionCreateIo>, IAnycmdCommand
    {
        public AddActionCommand(IUserSession userSession, IActionCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
