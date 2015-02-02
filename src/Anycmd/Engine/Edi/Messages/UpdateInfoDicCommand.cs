
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateInfoDicCommand : UpdateEntityCommand<IInfoDicUpdateIo>, IAnycmdCommand
    {
        public UpdateInfoDicCommand(IAcSession userSession, IInfoDicUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
