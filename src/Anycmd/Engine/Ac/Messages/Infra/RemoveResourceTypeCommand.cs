
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using Model;
    using System;

    public class RemoveResourceTypeCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveResourceTypeCommand(Guid resourceTypeId)
            : base(resourceTypeId)
        {

        }
    }
}
