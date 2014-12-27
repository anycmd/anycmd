
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveOrganizationActionCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveOrganizationActionCommand(Guid ontologyOrganizationActionId)
            : base(ontologyOrganizationActionId)
        {

        }
    }
}
