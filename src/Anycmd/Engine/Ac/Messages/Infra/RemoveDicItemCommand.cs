
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveDicItemCommand : RemoveEntityCommand
    {
        public RemoveDicItemCommand(IAcSession acSession, Guid dicItemId)
            : base(acSession, dicItemId)
        {

        }
    }
}
