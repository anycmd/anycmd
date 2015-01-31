
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class ChangeProcessCatalogCommand : Command, IAnycmdCommand
    {
        public ChangeProcessCatalogCommand(IUserSession userSession, Guid processId, string catalogCode)
        {
            this.UserSession = userSession;
            this.ProcessId = processId;
            this.CatalogCode = catalogCode;
        }

        public IUserSession UserSession { get; private set; }

        public Guid ProcessId { get; private set; }
        public string CatalogCode { get; private set; }
    }
}
