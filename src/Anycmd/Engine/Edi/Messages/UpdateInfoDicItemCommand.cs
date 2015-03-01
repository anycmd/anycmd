
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class UpdateInfoDicItemCommand : UpdateEntityCommand<IInfoDicItemUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoDicItemCommand(IAcSession acSession, IInfoDicItemUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
