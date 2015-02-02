
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveCatalogCommand : RemoveEntityCommand
    {
        public RemoveCatalogCommand(IAcSession userSession, Guid catalogId)
            : base(userSession, catalogId)
        {

        }
    }
}
