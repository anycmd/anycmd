
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public sealed class RemoveCatalogActionCommand : RemoveEntityCommand
    {
        public RemoveCatalogActionCommand(IAcSession acSession, Guid ontologyCatalogActionId)
            : base(acSession, ontologyCatalogActionId)
        {

        }
    }
}
