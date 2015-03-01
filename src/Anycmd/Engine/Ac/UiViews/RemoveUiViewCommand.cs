
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;
    using System;

    public sealed class RemoveUiViewCommand : RemoveEntityCommand
    {
        public RemoveUiViewCommand(IAcSession acSession, Guid viewId)
            : base(acSession, viewId)
        {

        }
    }
}
