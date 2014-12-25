using Anycmd.Commands;
using System;

namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    public class AddCommonPropertiesCommand : Command, ISysCommand
    {
        public AddCommonPropertiesCommand(Guid entityTypeId)
        {
            this.EntityTypeId = entityTypeId;
        }

        public Guid EntityTypeId { get; private set; }
    }
}
