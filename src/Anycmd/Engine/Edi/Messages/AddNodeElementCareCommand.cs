﻿
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public class AddNodeElementCareCommand : AddEntityCommand<INodeElementCareCreateIo>, IAnycmdCommand
    {
        public AddNodeElementCareCommand(IAcSession acSession, INodeElementCareCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
