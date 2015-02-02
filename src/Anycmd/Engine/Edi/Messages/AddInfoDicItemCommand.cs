
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddInfoDicItemCommand : AddEntityCommand<IInfoDicItemCreateIo>, IAnycmdCommand
    {
        public AddInfoDicItemCommand(IAcSession acSession, IInfoDicItemCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
