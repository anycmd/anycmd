
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;
    using System;

    public sealed class RemoveButtonCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveButtonCommand(IAcSession acSession, Guid buttonId)
            : base(acSession, buttonId)
        {

        }
    }
}
