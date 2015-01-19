
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddBatchCommand : AddEntityCommand<IBatchCreateIo>, IAnycmdCommand
    {
        public AddBatchCommand(IUserSession userSession, IBatchCreateIo input)
            : base(userSession, input)
        {
        }
    }
}
