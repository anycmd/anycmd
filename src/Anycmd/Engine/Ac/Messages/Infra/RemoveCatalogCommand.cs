
namespace Anycmd.Engine.Ac.Messages.Infra
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
