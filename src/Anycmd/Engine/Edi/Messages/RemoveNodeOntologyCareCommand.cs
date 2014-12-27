
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveNodeOntologyCareCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveNodeOntologyCareCommand(Guid nodeOntologyCareId)
            : base(nodeOntologyCareId)
        {

        }
    }
}
