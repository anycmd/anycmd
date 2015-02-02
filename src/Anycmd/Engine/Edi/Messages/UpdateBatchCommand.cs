
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateBatchCommand : UpdateEntityCommand<IBatchUpdateIo>, IAnycmdCommand
    {
        public UpdateBatchCommand(IAcSession acSession, IBatchUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
