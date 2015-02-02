
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddInfoDicCommand : AddEntityCommand<IInfoDicCreateIo>, IAnycmdCommand
    {
        public AddInfoDicCommand(IAcSession acSession, IInfoDicCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
