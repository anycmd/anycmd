
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateBatchCommand : UpdateEntityCommand<IBatchUpdateIo>, IAnycmdCommand
    {
        public UpdateBatchCommand(IAcSession userSession, IBatchUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
