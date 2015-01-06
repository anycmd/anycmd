
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveOntologyCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveOntologyCommand(Guid ontologyId)
            : base(ontologyId)
        {

        }
    }
}
