
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveDicCommand : RemoveEntityCommand
    {
        public RemoveDicCommand(IAcSession acSession, Guid dicId)
            : base(acSession, dicId)
        {

        }
    }
}
