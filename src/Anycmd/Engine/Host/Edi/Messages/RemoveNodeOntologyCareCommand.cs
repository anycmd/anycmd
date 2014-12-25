
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveNodeOntologyCareCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveNodeOntologyCareCommand(Guid nodeOntologyCareId)
            : base(nodeOntologyCareId)
        {

        }
    }
}
