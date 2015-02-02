
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveMenuCommand : RemoveEntityCommand
    {
        public RemoveMenuCommand(IAcSession acSession, Guid menuId)
            : base(acSession, menuId)
        {

        }
    }
}
