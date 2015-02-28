
namespace Anycmd.Engine.Ac.Catalogs
{
    using Messages;
    using System;

    public sealed class RemoveCatalogCommand : RemoveEntityCommand
    {
        public RemoveCatalogCommand(IAcSession acSession, Guid catalogId)
            : base(acSession, catalogId)
        {

        }
    }
}
