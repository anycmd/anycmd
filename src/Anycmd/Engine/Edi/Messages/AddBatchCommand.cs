
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddBatchCommand : AddEntityCommand<IBatchCreateIo>, ISysCommand
    {
        public AddBatchCommand(IBatchCreateIo input)
            : base(input)
        {

        }
    }
}
