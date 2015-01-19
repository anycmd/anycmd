
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveButtonCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveButtonCommand(IUserSession userSession, Guid buttonId)
            : base(userSession, buttonId)
        {

        }
    }
}
