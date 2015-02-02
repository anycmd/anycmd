
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveMenuCommand : RemoveEntityCommand
    {
        public RemoveMenuCommand(IAcSession userSession, Guid menuId)
            : base(userSession, menuId)
        {

        }
    }
}
