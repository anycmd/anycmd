
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateInfoDicCommand : UpdateEntityCommand<IInfoDicUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoDicCommand(IUserSession userSession, IInfoDicUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
