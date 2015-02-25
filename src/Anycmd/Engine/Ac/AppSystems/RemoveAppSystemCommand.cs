
namespace Anycmd.Engine.Ac.AppSystems
{
    using System;

    public class RemoveAppSystemCommand : RemoveEntityCommand
    {
        public RemoveAppSystemCommand(IAcSession acSession, Guid appSystemId)
            : base(acSession, appSystemId)
        {

        }
    }
}
