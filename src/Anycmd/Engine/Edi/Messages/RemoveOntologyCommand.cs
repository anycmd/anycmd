
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveOntologyCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveOntologyCommand(Guid ontologyId)
            : base(ontologyId)
        {

        }
    }
}
