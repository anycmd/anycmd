using Anycmd.Commands;
using System;

namespace Anycmd.Engine.Ac.Messages.Infra
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
