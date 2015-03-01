
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class AddInfoDicItemCommand : AddEntityCommand<IInfoDicItemCreateIo>, IAnycmdCommand
    {
        public AddInfoDicItemCommand(IAcSession acSession, IInfoDicItemCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
