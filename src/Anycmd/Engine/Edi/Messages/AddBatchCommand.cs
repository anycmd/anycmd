
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public class AddBatchCommand : AddEntityCommand<IBatchCreateIo>, IAnycmdCommand
    {
        public AddBatchCommand(IAcSession acSession, IBatchCreateIo input)
            : base(acSession, input)
        {
        }
    }
}
