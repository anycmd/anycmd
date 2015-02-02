
namespace Anycmd.Engine.Ac.Messages.Infra
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
