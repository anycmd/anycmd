
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class UpdateBatchCommand : UpdateEntityCommand<IBatchUpdateIo>, IAnycmdCommand
    {
        public UpdateBatchCommand(IAcSession acSession, IBatchUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
