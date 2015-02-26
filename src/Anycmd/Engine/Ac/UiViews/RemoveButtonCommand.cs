
namespace Anycmd.Engine.Ac.UiViews
{
    using System;

    public class RemoveButtonCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveButtonCommand(IAcSession acSession, Guid buttonId)
            : base(acSession, buttonId)
        {

        }
    }
}
