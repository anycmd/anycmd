
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddElementCommand : AddEntityCommand<IElementCreateIo>, IAnycmdCommand
    {
        public AddElementCommand(IAcSession userSession, IElementCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
