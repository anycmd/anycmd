
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateBatchCommand : UpdateEntityCommand<IBatchUpdateIo>, IAnycmdCommand
    {
        public UpdateBatchCommand(IUserSession userSession, IBatchUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
