﻿
namespace Anycmd.Engine.Ac.Groups
{
    using Messages;

    public class UpdateGroupCommand : UpdateEntityCommand<IGroupUpdateIo>, IAnycmdCommand
    {
        public UpdateGroupCommand(IAcSession acSession, IGroupUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
