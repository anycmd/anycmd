
namespace Anycmd.Engine.Ac.UiViews
{
    using System;

    public class RemoveUiViewButtonCommand : RemoveEntityCommand
    {
        public RemoveUiViewButtonCommand(IAcSession acSession, Guid viewButtonId)
            : base(acSession, viewButtonId)
        {

        }
    }
}
