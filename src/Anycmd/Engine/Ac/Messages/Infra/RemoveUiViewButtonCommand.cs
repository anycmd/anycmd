
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveUiViewButtonCommand : RemoveEntityCommand
    {
        public RemoveUiViewButtonCommand(IUserSession userSession, Guid viewButtonId)
            : base(userSession, viewButtonId)
        {

        }
    }
}
