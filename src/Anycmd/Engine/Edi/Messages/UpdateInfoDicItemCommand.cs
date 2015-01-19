
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateInfoDicItemCommand : UpdateEntityCommand<IInfoDicItemUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoDicItemCommand(IUserSession userSession, IInfoDicItemUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
