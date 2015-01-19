
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveOntologyCommand : RemoveEntityCommand
    {
        public RemoveOntologyCommand(IUserSession userSession, Guid ontologyId)
            : base(userSession, ontologyId)
        {

        }
    }
}
