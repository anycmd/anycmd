
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveButtonCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveButtonCommand(IAcSession userSession, Guid buttonId)
            : base(userSession, buttonId)
        {

        }
    }
}
