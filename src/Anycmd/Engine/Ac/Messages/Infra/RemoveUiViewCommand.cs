
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveUiViewCommand : RemoveEntityCommand
    {
        public RemoveUiViewCommand(IAcSession userSession, Guid viewId)
            : base(userSession, viewId)
        {

        }
    }
}
