
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;
    using System;

    public class RemoveMenuCommand : RemoveEntityCommand
    {
        public RemoveMenuCommand(IAcSession acSession, Guid menuId)
            : base(acSession, menuId)
        {

        }
    }
}
