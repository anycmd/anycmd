
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveOntologyCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveOntologyCommand(Guid ontologyId)
            : base(ontologyId)
        {

        }
    }
}
