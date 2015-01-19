
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveUiViewCommand : RemoveEntityCommand
    {
        public RemoveUiViewCommand(IUserSession userSession, Guid viewId)
            : base(userSession, viewId)
        {

        }
    }
}
