
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;
    using System;

    public class AddBatchCommand : AddEntityCommand<IBatchCreateIo>, IAnycmdCommand
    {
        public AddBatchCommand(IBatchCreateIo input, IUserSession userSession)
            : base(input)
        {
            if (userSession == null)
            {
                throw new ArgumentNullException("userSession");
            }
            this.UserSession = userSession;
        }

        public IUserSession UserSession { get; private set; }
    }
}
