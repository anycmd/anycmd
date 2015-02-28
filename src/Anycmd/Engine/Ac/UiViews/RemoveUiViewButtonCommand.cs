
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;
    using System;

    public class RemoveUiViewButtonCommand : RemoveEntityCommand
    {
        public RemoveUiViewButtonCommand(IAcSession acSession, Guid viewButtonId)
            : base(acSession, viewButtonId)
        {

        }
    }
}
