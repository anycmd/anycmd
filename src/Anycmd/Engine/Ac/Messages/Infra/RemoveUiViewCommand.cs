
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveUiViewCommand : RemoveEntityCommand
    {
        public RemoveUiViewCommand(IAcSession acSession, Guid viewId)
            : base(acSession, viewId)
        {

        }
    }
}
