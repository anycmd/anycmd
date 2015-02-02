
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveOntologyCommand : RemoveEntityCommand
    {
        public RemoveOntologyCommand(IAcSession userSession, Guid ontologyId)
            : base(userSession, ontologyId)
        {

        }
    }
}
