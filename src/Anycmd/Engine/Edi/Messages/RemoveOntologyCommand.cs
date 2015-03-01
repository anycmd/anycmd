
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public sealed class RemoveOntologyCommand : RemoveEntityCommand
    {
        public RemoveOntologyCommand(IAcSession acSession, Guid ontologyId)
            : base(acSession, ontologyId)
        {

        }
    }
}
