
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveOntologyCommand : RemoveEntityCommand
    {
        public RemoveOntologyCommand(IAcSession acSession, Guid ontologyId)
            : base(acSession, ontologyId)
        {

        }
    }
}
