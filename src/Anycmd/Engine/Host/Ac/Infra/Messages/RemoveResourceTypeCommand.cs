
namespace Anycmd.Engine.Host.Ac.Infra.Messages
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
