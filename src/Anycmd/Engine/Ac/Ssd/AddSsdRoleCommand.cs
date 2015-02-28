﻿
namespace Anycmd.Engine.Ac.Ssd
{
    using Messages;

    public class AddSsdRoleCommand : AddEntityCommand<ISsdRoleCreateIo>, IAnycmdCommand
    {
        public AddSsdRoleCommand(IAcSession acSession, ISsdRoleCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
