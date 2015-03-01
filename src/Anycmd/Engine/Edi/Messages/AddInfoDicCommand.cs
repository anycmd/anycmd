
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class AddInfoDicCommand : AddEntityCommand<IInfoDicCreateIo>, IAnycmdCommand
    {
        public AddInfoDicCommand(IAcSession acSession, IInfoDicCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
