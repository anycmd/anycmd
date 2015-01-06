
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveOrganizationActionCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveOrganizationActionCommand(Guid ontologyOrganizationActionId)
            : base(ontologyOrganizationActionId)
        {

        }
    }
}
