
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveOrganizationActionCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveOrganizationActionCommand(Guid ontologyOrganizationActionId)
            : base(ontologyOrganizationActionId)
        {

        }
    }
}
