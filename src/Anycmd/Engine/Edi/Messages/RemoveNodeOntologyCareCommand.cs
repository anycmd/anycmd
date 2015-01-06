
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveNodeOntologyCareCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveNodeOntologyCareCommand(Guid nodeOntologyCareId)
            : base(nodeOntologyCareId)
        {

        }
    }
}
