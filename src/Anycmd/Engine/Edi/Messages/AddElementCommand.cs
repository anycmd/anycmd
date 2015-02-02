
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddElementCommand : AddEntityCommand<IElementCreateIo>, IAnycmdCommand
    {
        public AddElementCommand(IAcSession acSession, IElementCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
