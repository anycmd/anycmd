
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateInfoDicItemCommand : UpdateEntityCommand<IInfoDicItemUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoDicItemCommand(IAcSession acSession, IInfoDicItemUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
