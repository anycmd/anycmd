
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveUiViewButtonCommand : RemoveEntityCommand
    {
        public RemoveUiViewButtonCommand(IAcSession userSession, Guid viewButtonId)
            : base(userSession, viewButtonId)
        {

        }
    }
}
