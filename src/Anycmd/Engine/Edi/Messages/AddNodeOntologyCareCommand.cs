﻿
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using InOuts;

    public class AddNodeOntologyCareCommand: AddEntityCommand<INodeOntologyCareCreateIo>, IAnycmdCommand
    {
        public AddNodeOntologyCareCommand(IAcSession acSession, INodeOntologyCareCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
