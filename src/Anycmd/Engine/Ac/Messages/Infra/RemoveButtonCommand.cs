
namespace Anycmd.Engine.Ac.Messages.Infra
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
