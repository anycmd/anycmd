
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveDicCommand : RemoveEntityCommand
    {
        public RemoveDicCommand(IAcSession userSession, Guid dicId)
            : base(userSession, dicId)
        {

        }
    }
}
