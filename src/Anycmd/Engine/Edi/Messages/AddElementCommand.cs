
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class AddElementCommand : AddEntityCommand<IElementCreateIo>, IAnycmdCommand
    {
        public AddElementCommand(IAcSession acSession, IElementCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
