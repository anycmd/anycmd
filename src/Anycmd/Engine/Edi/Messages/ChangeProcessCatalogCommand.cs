
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class ChangeProcessCatalogCommand : Command, IAnycmdCommand
    {
        public ChangeProcessCatalogCommand(IAcSession userSession, Guid processId, string catalogCode)
        {
            this.AcSession = userSession;
            this.ProcessId = processId;
            this.CatalogCode = catalogCode;
        }

        public IAcSession AcSession { get; private set; }

        public Guid ProcessId { get; private set; }
        public string CatalogCode { get; private set; }
    }
}
