
namespace Anycmd.Engine.Ac.Catalogs
{
    using System;

    public class RemoveCatalogCommand : RemoveEntityCommand
    {
        public RemoveCatalogCommand(IAcSession acSession, Guid catalogId)
            : base(acSession, catalogId)
        {

        }
    }
}
