
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public sealed class AddBatchCommand : AddEntityCommand<IBatchCreateIo>, IAnycmdCommand
    {
        public AddBatchCommand(IAcSession acSession, IBatchCreateIo input)
            : base(acSession, input)
        {
        }
    }
}
