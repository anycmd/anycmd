
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateBatchCommand : UpdateEntityCommand<IBatchUpdateIo>, ISysCommand
    {
        public UpdateBatchCommand(IBatchUpdateIo input)
            : base(input)
        {

        }
    }
}
