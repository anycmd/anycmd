﻿
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class RemoveOntologyCatalogCommand : Command, IAnycmdCommand
    {
        public RemoveOntologyCatalogCommand(IAcSession userSession, Guid ontologyId, Guid catalogId)
        {
            this.AcSession = userSession;
            this.OntologyId = ontologyId;
            this.CatalogId = catalogId;
        }

        public IAcSession AcSession { get; private set; }

        public Guid OntologyId { get; private set; }

        public Guid CatalogId { get; private set; }
    }
}
