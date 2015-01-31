
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveCatalogCommand : RemoveEntityCommand
    {
        public RemoveCatalogCommand(IUserSession userSession, Guid catalogId)
            : base(userSession, catalogId)
        {

        }
    }
}
