﻿
namespace Anycmd.Engine.Ac.Functions
{
    using Messages;

    public class AddFunctionCommand : AddEntityCommand<IFunctionCreateIo>, IAnycmdCommand
    {
        public AddFunctionCommand(IAcSession acSession, IFunctionCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
