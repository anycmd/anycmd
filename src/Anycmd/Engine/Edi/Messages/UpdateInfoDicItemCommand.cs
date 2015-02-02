
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateInfoDicItemCommand : UpdateEntityCommand<IInfoDicItemUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoDicItemCommand(IAcSession userSession, IInfoDicItemUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
