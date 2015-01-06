
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateBatchCommand : UpdateEntityCommand<IBatchUpdateIo>, IAnycmdCommand
    {
        public UpdateBatchCommand(IBatchUpdateIo input)
            : base(input)
        {

        }
    }
}
