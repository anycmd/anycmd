
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public sealed class RemoveNodeOntologyCareCommand : RemoveEntityCommand
    {
        public RemoveNodeOntologyCareCommand(IAcSession acSession, Guid nodeOntologyCareId)
            : base(acSession, nodeOntologyCareId)
        {

        }
    }
}
