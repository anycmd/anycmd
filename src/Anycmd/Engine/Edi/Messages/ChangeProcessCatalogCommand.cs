
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class ChangeProcessCatalogCommand : Command, IAnycmdCommand
    {
        public ChangeProcessCatalogCommand(IAcSession acSession, Guid processId, string catalogCode)
        {
            this.AcSession = acSession;
            this.ProcessId = processId;
            this.CatalogCode = catalogCode;
        }

        public IAcSession AcSession { get; private set; }

        public Guid ProcessId { get; private set; }
        public string CatalogCode { get; private set; }
    }
}
