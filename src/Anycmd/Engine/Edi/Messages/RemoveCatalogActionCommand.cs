
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveCatalogActionCommand : RemoveEntityCommand
    {
        public RemoveCatalogActionCommand(IUserSession userSession, Guid ontologyCatalogActionId)
            : base(userSession, ontologyCatalogActionId)
        {

        }
    }
}
