
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveNodeOntologyCareCommand : RemoveEntityCommand
    {
        public RemoveNodeOntologyCareCommand(IAcSession userSession, Guid nodeOntologyCareId)
            : base(userSession, nodeOntologyCareId)
        {

        }
    }
}
